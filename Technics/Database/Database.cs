using Dapper;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Technics.Properties;
using static Technics.Database;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database : DefaultInstance<Database>
    {
        private ConnectionSQLite Connection { get; set; } = new ConnectionSQLite();

        public string FileName
        {
            get => Connection.FileName;
            set
            {
                Connection.FileName = value;

                CreateDatabase();
            }
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(Connection.ConnectionString);
        }

        private void CreateDatabase()
        {
            if (File.Exists(FileName)) return;

            SQLiteConnection.CreateFile(FileName);

            using (var connection = GetConnection())
            {
                /* tables */
                connection.Execute(ResourcesSql.CreateTableParts);
                connection.Execute(ResourcesSql.CreateTableTechs);
                connection.Execute(ResourcesSql.CreateTableFolders);
                connection.Execute(ResourcesSql.CreateTableMileages);
                connection.Execute(ResourcesSql.CreateTableTechParts);
            }

            Utils.Log.Info(ResourcesLog.DatabaseCreateOk);
        }

#if DEBUG
        private async Task TruncateTableAsync<T>(DbConnection connection)
        {
            var tableName = Sql.TableName<T>();

            var sql = string.Format(ResourcesSql.TruncateTable, tableName);

            await Actions.ExecuteAsync(connection, sql);

            Utils.Log.Info($"truncate table {tableName} ok");
        }

        public async Task TruncateTableAsync<T>()
        {
            using (var connection = GetConnection())
            {
                await TruncateTableAsync<T>(connection);
            }
        }
#endif

        public async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, value, null);
            }

            Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(T).Name));
        }

#if DEBUG
        public async Task ListItemSaveAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, values);
            }

            DebugWrite.Line($"{typeof(T).Name} items (count={values?.Count()}) save ok");
        }
#endif

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, value, null);
            }

            Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name));
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, values);
            }

            var count = values?.Count();

            if (count > 1)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemListDeleteOk, typeof(T).Name, count));
            }
            else
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name));
            }
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql = null, object param = null)
        {
            var result = Enumerable.Empty<T>();

            using (var connection = GetConnection())
            {
                result = await Actions.ListLoadAsync<T>(connection, sql, param);
            }

            Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, result.Count()));

            return result;
        }

        public async Task<double> GetMileageCommonPrevAsync(MileageModel mileage)
        {
            var query = new Query()
            {
                Fields = "IFNULL(SUM(mileage), 0.0)",
                Table = Tables.mileages,
                Where = "id != :id AND techid = :techid AND datetime <= :datetime"
            };

            object param = new { id = mileage.Id, techid = mileage.TechId, datetime = mileage.DateTime };

            using (var connection = GetConnection())
            {
                return await Actions.QueryFirstOrDefaultAsync<double>(connection, query, param);
            }
        }

        private async Task<double> GetTechPartMileageAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            return await Actions.QueryFirstOrDefaultAsync<double>(connection,
                ResourcesSql.GetTechPartMileage,
                    new
                    {
                        techid = techPart.TechId,
                        datetimeinstall = techPart.DateTimeInstall,
                        datetimeremove = techPart.DateTimeRemove ?? DateTime.Today.AddDays(1)
                    },
                transaction);
        }

        private async Task<double> GetTechPartMileageAsync(TechPartModel techPart)
        {
            using (var connection = GetConnection())
            {
                return await GetTechPartMileageAsync(connection, null, techPart);
            }
        }

        public string GetMileagesSql(IEnumerable<TechModel> techs)
        {
            var filter = new Filter.Mileages()
            {
                Techs = techs
            };

            var where = filter.ToString();

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return sql;
        }

        public string GetTechPartsSql(Filter.TechParts filter)
        {
            var where = filter.ToString();

            var sql = string.Format(ResourcesSql.SelectTechParts, where);

            return sql;
        }

        public string GetTechPartsSql(IEnumerable<TechModel> techs)
        {
            var filter = new Filter.TechParts()
            {
                Techs = techs
            };

            return GetTechPartsSql(filter);
        }

        private Query GetMileagesGetChangedQuery()
        {
            return new Query()
            {
                Fields = "id, techid, datetime",
                Table = Tables.mileages,
            };
        }

        private async Task<MileageModel> MileageLoadAsync(
            DbConnection connection, DbTransaction transaction, long id)
        {
            var query = GetMileagesGetChangedQuery();

            query.Where = $"id = :id";

            object param = new { id };

            return await Actions.QueryFirstOrDefaultAsync<MileageModel>(connection, query, param, transaction);
        }

        private async Task<IEnumerable<MileageModel>> MileagesGetChangedAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            if (mileage.TechId == null) return Enumerable.Empty<MileageModel>();

            var query = GetMileagesGetChangedQuery();

            query.Where = $"techid = :techid AND datetime > :datetime";

            object param = new
            {
                techid = mileage.TechId,
                datetime = mileage.DateTime,
            };

            return await Actions.ListLoadAsync<MileageModel>(connection, query, param, transaction);
        }

        private async Task<IEnumerable<MileageModel>> MileagesGetChangedAfterNewAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            return await MileagesGetChangedAsync(connection, transaction, mileage);
        }

        private async Task<IEnumerable<MileageModel>> MileagesGetChangedAfterDeleteAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            return await MileagesGetChangedAsync(connection, transaction, mileage);
        }

        private async Task<IEnumerable<MileageModel>> MileagesGetChangedAfterChangeAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            if (mileage.IsNew)
            {
                return await MileagesGetChangedAfterNewAsync(connection, transaction, mileage);
            }

            var currentMileage = await MileageLoadAsync(connection, transaction, mileage.Id);

            var changed = new List<MileageModel>();

            IEnumerable<MileageModel> changes;

            if (currentMileage.TechId != mileage.TechId ||
                currentMileage.DateTime != mileage.DateTime)
            {
                changes = await MileagesGetChangedAsync(connection, transaction, currentMileage);

                changed.AddRange(changes);
            }

            changes = await MileagesGetChangedAsync(connection, transaction, mileage);

            changed.AddRange(changes);

            return changed;
        }

        private async Task<List<ChangeMileageModel>> MileagesUpdateMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<MileageModel> mileages)
        {
            var changed = new List<ChangeMileageModel>();

            DebugWrite.Line($"count={mileages.Count()}");

            foreach (var mileage in mileages)
            {
                mileage.MileageCommon = await Actions.QueryFirstOrDefaultAsync<double>(connection,
                    ResourcesSql.GetMileageCommon,
                    new { techid = mileage.TechId, datetime = mileage.DateTime }, transaction);

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateMileagesMileageCommonById,
                    new { id = mileage.Id, mileagecommon = mileage.MileageCommon }, transaction);

                changed.Add(new ChangeMileageModel()
                {
                    Id = mileage.Id,
                    MileageCommon = mileage.MileageCommon,
                });
            }

            return changed;
        }

        private async Task<List<ChangeTechPartModel>> TechPartsUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var changed = new List<ChangeTechPartModel>(); return changed;

            if (mileage.TechId == null) return changed;

            var query = new Query()
            {
                Fields = "id, techid, partid, datetimeinstall, datetimeremove",
                Table = Tables.techparts,
                Where = $"techid = :techid AND datetimeremove >= :datetime"
            };

            object param = new
            {
                techid = mileage.TechId,
                datetime = mileage.DateTime,
            };

            var techPartsChangedList = await Actions.ListLoadAsync<TechPartModel>(connection, query, param, transaction);

            DebugWrite.Line($"count={techPartsChangedList.Count()}");

            foreach (var techPart in techPartsChangedList)
            {
                DebugWrite.Line(JsonConvert.SerializeObject(techPart));

                techPart.Mileage = await GetTechPartMileageAsync(connection, transaction, techPart);

                param = new { id = techPart.Id, mileage = techPart.Mileage };

                DebugWrite.Line(param);

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateTechPartsMileageById, param,
                    transaction);

                changed.Add(new ChangeTechPartModel()
                {
                    Id = techPart.Id,
                    Mileage = techPart.Mileage,
                });
            }

            return changed;
        }

        public async Task<ChangeModel> MileageSaveAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var changes = new ChangeModel();

                        var changed = await MileagesGetChangedAfterChangeAsync(connection, transaction, mileage);

                        await Actions.ListItemSaveAsync(connection, mileage, transaction);

                        changes.Mileages = await MileagesUpdateMileageCommonAsync(connection, transaction, changed);

                        changes.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, mileage);

                        transaction.Commit();

                        return changes;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<ChangeModel> MileageDeleteAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var changes = new ChangeModel();

                        var changed = await MileagesGetChangedAfterDeleteAsync(connection, transaction, mileage);

                        await Actions.ListItemDeleteAsync(connection, mileage, transaction);

                        changes.Mileages = await MileagesUpdateMileageCommonAsync(connection, transaction, changed);

                        changes.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, mileage);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(MileageModel).Name));

                        return changes;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task TechDeleteAsync(TechModel tech)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await Actions.ExecuteAsync(connection,
                            ResourcesSql.UpdateMileagesMileageCommonByTechId,
                            new { techid = tech.Id }, transaction);

                        await Actions.ListItemDeleteAsync(connection, tech, transaction);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(TechModel).Name));
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
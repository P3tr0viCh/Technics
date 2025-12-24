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

        private async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param)
        {
            using (var connection = GetConnection())
            {
                return await Actions.QueryFirstOrDefaultAsync<T>(connection, sql, param);
            }
        }

        public async Task<double> GetMileageCommonPrevAsync(MileageModel mileage)
        {
            return await QueryFirstOrDefaultAsync<double>(ResourcesSql.GetMileageCommonPrev,
                new { techid = mileage.TechId, datetime = mileage.DateTime });
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

        private async Task<IEnumerable<MileageModel>> MileagesUpdateMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, DateTime dateTime)
        {
            var mileageChangedList = await Actions.ListLoadAsync<MileageModel>(connection,
                ResourcesSql.SelectMileagesChangedByDateTime,
                new { datetime = dateTime }, transaction);

            DebugWrite.Line($"count={mileageChangedList.Count()}");

            foreach (var mileage in mileageChangedList)
            {
                mileage.MileageCommon = await Actions.QueryFirstOrDefaultAsync<double>(connection,
                    ResourcesSql.GetMileageCommon,
                    new { techid = mileage.TechId, datetime = mileage.DateTime }, transaction);

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateMileagesMileageCommonById,
                    new { id = mileage.Id, mileagecommon = mileage.MileageCommon }, transaction);
            }

            return mileageChangedList;
        }

        private async Task TechPartsUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            if (mileage.TechId == null) return;

            var query = new Query()
            {
                Fields = "id, techid, partid, datetimeinstall, datetimeremove",
                Table = Tables.techparts,
                Where = $"techid = {mileage.TechId}"
            };

            var techPartsChangedList = await Actions.ListLoadAsync<TechPartModel>(connection, query, transaction);

            DebugWrite.Line($"count={techPartsChangedList.Count()}");

            foreach (var techPart in techPartsChangedList)
            {
                DebugWrite.Line(JsonConvert.SerializeObject(techPart));

                techPart.Mileage = await GetTechPartMileageAsync(connection, transaction, techPart);

                var param = new { id = techPart.Id, mileage = techPart.Mileage };

                DebugWrite.Line(param);

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateTechPartsMileageById, param,
                    transaction);
            }
        }

        public async Task<IEnumerable<MileageModel>> MileageSaveAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await Actions.ListItemSaveAsync(connection, mileage, transaction);

                        var changedList = await MileagesUpdateMileageCommonAsync(connection, transaction, mileage.DateTime);

                        await TechPartsUpdateMileagesAsync(connection, transaction, mileage);

                        transaction.Commit();

                        return changedList;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<MileageModel>> MileageDeleteAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await Actions.ListItemDeleteAsync(connection, mileage, transaction);

                        var changedList = await MileagesUpdateMileageCommonAsync(connection, transaction, mileage.DateTime);

                        await TechPartsUpdateMileagesAsync(connection, transaction, mileage);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(MileageModel).Name));

                        return changedList;
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
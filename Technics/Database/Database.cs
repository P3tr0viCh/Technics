using Dapper;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
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
        }

        private async Task TruncateTableAsync<T>(DbConnection connection)
        {
            var sql = string.Format(ResourcesSql.TruncateTable, Sql.TableName<T>());

            await Actions.ExecuteAsync(connection, sql);
        }

        public async Task TruncateTableAsync<T>()
        {
            using (var connection = GetConnection())
            {
                await TruncateTableAsync<T>(connection);
            }
        }

        public async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, value, null);
            }
        }

#if DEBUG
        public async Task ListItemSaveAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, values);
            }
        }
#endif

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, value, null);
            }
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, values);
            }
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql = null, object param = null)
        {
            using (var connection = GetConnection())
            {
                return await Actions.ListLoadAsync<T>(connection, sql, param);
            }
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

        public async Task<double> GetTechPartMileageAsync(TechPartModel techPart)
        {
            return await QueryFirstOrDefaultAsync<double>(ResourcesSql.GetTechPartMileage,
                new
                {
                    techid = techPart.TechId,
                    datetimeinstall = techPart.DateTimeInstall,
                    datetimeremove = techPart.DateTimeRemove ?? DateTime.Today.AddDays(1)
                });
        }

        public string GetMileagesSql(IEnumerable<TechModel> techs)
        {
            var filter = new Filter.Mileages()
            {
                Techs = techs
            };

            var where = Filter.GetWhereSql(filter);

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return sql;
        }

        public string GetTechPartsSql(Filter.TechParts filter)
        {
            var where = Filter.GetWhereSql(filter);

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
            var list = await Actions.ListLoadAsync<MileageModel>(connection,
                ResourcesSql.SelectMileagesChangedByDateTime,
                new { datetime = dateTime }, transaction);

            DebugWrite.Line($"count={list.Count()}");

            foreach (var item in list)
            {
                item.MileageCommon = await Actions.QueryFirstOrDefaultAsync<double>(connection,
                    ResourcesSql.GetMileageCommon,
                    new { techid = item.TechId, datetime = item.DateTime }, transaction);

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateMileagesMileageCommonById,
                    new { id = item.Id, mileagecommon = item.MileageCommon }, transaction);
            }

            return list;
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

        public async Task TechDeleteAsync(TechModel tech)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await Actions.ListItemDeleteAsync(connection, tech, transaction);

                        await Actions.ExecuteAsync(connection,
                            ResourcesSql.UpdateMileagesMileageCommonByTechId,
                            new { techid = tech.Id }, transaction);

                        transaction.Commit();
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
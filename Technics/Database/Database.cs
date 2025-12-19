#if DEBUG
#define _SHOW_SQL
#endif

using Dapper;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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

        public async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, null, value);
            }
        }

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, null, value);
            }
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var value in values)
                        {
                            await Actions.ListItemDeleteAsync(connection, transaction, value);
                        }

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

        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql)
        {
            using (var connection = GetConnection())
            {
                return await Actions.ListLoadAsync<T>(connection, sql);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param)
        {
            using (var connection = GetConnection())
            {
#if SHOW_SQL
                DebugWrite.Line(sql.ReplaceEol());
                DebugWrite.Line(param);
#endif

                try
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
                }
                catch (Exception e)
                {
                    e.AddQuery(sql);
                    throw;
                }
            }
        }

        private async Task<double> GetMileageCommonInternalAsync(MileageModel mileage, string sql)
        {
            return await QueryFirstOrDefaultAsync<double>(sql,
                new { techid = mileage.TechId, datetime = mileage.DateTime });
        }

        public async Task<double> GetMileageCommonAsync(MileageModel mileage)
        {
            return await GetMileageCommonInternalAsync(mileage, ResourcesSql.GetMileageCommon);
        }

        public async Task<double> GetMileageCommonPrevAsync(MileageModel mileage)
        {
            return await GetMileageCommonInternalAsync(mileage, ResourcesSql.GetMileageCommonPrev);
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
    }
}
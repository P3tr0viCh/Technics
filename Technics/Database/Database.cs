using Dapper;
using Dapper.Contrib.Extensions;
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

                /* indexes */

                /* triggers */
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

        public async Task ListItemDeleteAsync<T>(List<T> values) where T : BaseId
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

        public async Task<List<T>> ListLoadAsync<T>(Query query)
        {
            using (var connection = GetConnection())
            {
                return await Actions.ListLoadAsync<T>(connection, query);
            }
        }

        public async Task<List<T>> ListLoadAsync<T>()
        {
            return await ListLoadAsync<T>(null);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
                }
                catch (Exception e)
                {
                    Sql.ExceptionAddQuery(e, sql);
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
    }
}
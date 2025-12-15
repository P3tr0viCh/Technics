using Dapper;
using Newtonsoft.Json.Linq;
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
                connection.Execute(ResourcesSql.CreateTableFolders);
                connection.Execute(ResourcesSql.CreateTableTechs);
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

        public async Task<List<T>> ListLoadAsync<T>()
        {
            using (var connection = GetConnection())
            {
                return await Actions.ListLoadAsync<T>(connection);
            }
        }

        public async Task<double> GetMileageCommonAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                var sql = ResourcesSql.GetMileageCommon;

                try
                {
                    return await connection.QueryFirstOrDefaultAsync<double>(sql,
                        new { techid = mileage.TechId, datetime = mileage.DateTime });
                }
                catch (Exception e)
                {
                    Sql.ExceptionAddQuery(e, sql);
                    throw;
                }
            }
        }
    }
}
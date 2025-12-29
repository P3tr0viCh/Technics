using Dapper;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
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

            Utils.Log.ListItemSaveOk<T>();
        }

        public async Task ListItemSaveAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemSaveAsync(connection, values);
            }

            Utils.Log.ListItemSaveOk(values);
        }

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, value, null);
            }

            Utils.Log.ListItemDeleteOk<T>();
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await Actions.ListItemDeleteAsync(connection, values);
            }

            Utils.Log.ListItemDeleteOk(values);
        }

        public async Task<T> ListItemLoadByIdAsync<T>(DbConnection connection, DbTransaction transaction, long id)
        {
            var query = new Query
            {
                Table = Sql.TableName<T>(),
                Where = $"id = :id"
            };

            object param = new { id };

            return await Actions.QueryFirstOrDefaultAsync<T>(connection, query, param, transaction);
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql = null, object param = null)
        {
            IEnumerable<T> list;

            using (var connection = GetConnection())
            {
                list = await Actions.ListLoadAsync<T>(connection, sql, param);
            }

            Utils.Log.LoadListOk(list);

            return list;
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
    }
}
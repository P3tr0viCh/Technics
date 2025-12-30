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

            await connection.ExecuteSqlAsync(sql);

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
                await connection.ListItemSaveAsync(value, null);
            }

            Utils.Log.ListItemSaveOk<T>();
        }

        public async Task ListItemSaveAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await connection.ListItemSaveAsync(values);
            }

            Utils.Log.ListItemSaveOk(values);
        }

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await connection.ListItemDeleteAsync(value, null);
            }

            Utils.Log.ListItemDeleteOk<T>();
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            using (var connection = GetConnection())
            {
                await connection.ListItemDeleteAsync(values);
            }

            Utils.Log.ListItemDeleteOk(values);
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql = null, object param = null)
        {
            IEnumerable<T> list;

            using (var connection = GetConnection())
            {
                list = await connection.ListLoadAsync<T>(sql, param);
            }

            Utils.Log.ListLoadOk(list);

            return list;
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>(Query query)
        {
            return await ListLoadAsync<T>(query.ToString());
        }
    }
}
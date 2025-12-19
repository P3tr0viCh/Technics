using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;

namespace Technics
{
    public partial class Main
    {
        public async Task<IEnumerable<T>> ListLoadAsync<T>(string sql) where T : BaseId
        {
            var result = await Database.Default.ListLoadAsync<T>(sql);

            Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, result.Count()));

            return result;
        }

        public async Task<IEnumerable<T>> ListLoadAsync<T>() where T : BaseId
        {
            return await ListLoadAsync<T>(string.Empty);
        }

        public async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemSaveAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(T).Name));
        }

        public async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemDeleteAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name));
        }

        public async Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
        {
            await Database.Default.ListItemDeleteAsync(values);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name, values.Count()));
        }
    }
}
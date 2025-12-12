using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Main
    {
        private async Task<List<T>> ListLoadAsync<T>()
        {
            var result = await Database.Default.ListLoadAsync<T>();

            Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, result.Count));

            return result;
        }

        private async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemSaveAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(T).Name));
        }

        private async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemDeleteAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name));
        }
    }
}
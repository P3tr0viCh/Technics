using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;

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
    }
}
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Main
    {
        private async Task<IEnumerable<T>> ListLoadAsync<T>(string sql)
        {
            var result = await Database.Default.ListLoadAsync<T>(sql);

            Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, result.Count()));

            return result;
        }

        private async Task<IEnumerable<T>> ListLoadAsync<T>()
        {
            return await ListLoadAsync<T>(string.Empty);
        }

        public async Task ListItemSaveAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemSaveAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(T).Name));
        }

        private async Task ListItemDeleteAsync<T>(T value) where T : BaseId
        {
            await Database.Default.ListItemDeleteAsync(value);

            Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(T).Name));
        }

        public string GetMileagesSql(List<TechModel> techs)
        {
            var where = string.Empty;

            if (techs.Count != Lists.Default.Techs.Count)
            {
                if (techs.Count == 1)
                {
                    where = $" = {techs[0].Id}";
                }
                else
                {
                    techs.ForEach(tech => where = where.JoinExcludeEmpty(", ", tech.Id.ToString()));

                    where = $" IN ({where})";
                }

                where = $"WHERE {Sql.FieldName(nameof(MileageModel.TechId)) + where}";
            }

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return sql;
        }
    }
}
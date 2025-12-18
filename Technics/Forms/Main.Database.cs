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

        public string GetWhereSql(IEnumerable<TechModel> techs)
        {
            var where = string.Empty;

            var techList = techs.ToList();

            if (techList.Count != Lists.Default.Techs.Count)
            {
                if (techList.Count == 1)
                {
                    where = $" = {techList[0].Id}";
                }
                else
                {
                    techList.ForEach(tech => where = where.JoinExcludeEmpty(", ", tech.Id.ToString()));

                    where = $" IN ({where})";
                }

                where = $"WHERE {Sql.FieldName(nameof(BaseTechId.TechId)) + where}";
            }

            return where;
        }

        public string GetMileagesSql(IEnumerable<TechModel> techs)
        {
            var where = GetWhereSql(techs);

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return sql;
        }

        public string GetTechPartsSql(IEnumerable<TechModel> techs)
        {
            var where = GetWhereSql(techs);

            var sql = string.Format(ResourcesSql.SelectTechParts, where);

            return sql;
        }
    }
}
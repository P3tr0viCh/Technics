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
        private async Task<IEnumerable<T>> ListLoadAsync<T>(string sql) where T : BaseId
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

        private string GetWhereSql(IEnumerable<TechModel> techs)
        {
            var where = string.Empty;

            var techList = techs.ToList();

            if (techList.Count != Lists.Default.Techs.Count)
            {
                var fieldName = Sql.FieldName(nameof(BaseTechId.TechId));

                if (techList.Count == 1)
                {
                    where = $"{fieldName} = {techList[0].Id}";
                }
                else
                {
                    techList.ForEach(tech => where = where.JoinExcludeEmpty(", ", tech.Id.ToString()));

                    where = $"{fieldName} IN ({where})";
                }
            }

            if (!where.IsEmpty())
            {
                where = $"WHERE {where}";
            }

            return where;
        }

        private string GetMileagesSql(IEnumerable<TechModel> techs)
        {
            var where = GetWhereSql(techs);

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return sql;
        }

        private string GetTechPartsSql(IEnumerable<TechModel> techs)
        {
            var where = GetWhereSql(techs);

            var sql = string.Format(ResourcesSql.SelectTechParts, where);

            return sql;
        }
    }
}
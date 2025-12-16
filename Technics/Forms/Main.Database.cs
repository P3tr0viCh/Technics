using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Main
    {
        private async Task<List<T>> ListLoadAsync<T>(Query query)
        {
            var result = await Database.Default.ListLoadAsync<T>(query);

            Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, result.Count));

            return result;
        }

        private async Task<List<T>> ListLoadAsync<T>()
        {
            return await ListLoadAsync<T>(null);
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

        public Query GetMileagesQuery(List<TechModel> techs)
        {
            var query = new Query()
            {
                Table = Database.Tables.mileages,
                Order = $"{Sql.FieldName(nameof(MileageModel.DateTime))} DESC",
            };

            if (techs.Count != Lists.Default.Techs.Count)
            {
                var where = string.Empty;

                if (techs.Count == 1)
                {
                    where = $" = {techs[0].Id}";
                }
                else
                {
                    techs.ForEach(tech => where = where.JoinExcludeEmpty(", ", tech.Id.ToString()));

                    where = $" IN ({where})";
                }

                where = Sql.FieldName(nameof(MileageModel.TechId)) + where;

                query.Where = where;
            }

            return query;
        }
    }
}
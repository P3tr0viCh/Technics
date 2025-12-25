using P3tr0viCh.Database;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        private async Task TechDeleteAsync(
            DbConnection connection, DbTransaction transaction, TechModel tech)
        {
            await Actions.ExecuteAsync(connection,
                ResourcesSql.ClearMileagesMileageCommonByTechId,
                new { techid = tech.Id }, transaction);

            await Actions.ExecuteAsync(connection,
                ResourcesSql.ClearTechPartsMileagesByTechId,
                new { techid = tech.Id }, transaction);

            await Actions.ListItemDeleteAsync(connection, tech, transaction);
        }

        public async Task TechDeleteAsync(TechModel tech)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await TechDeleteAsync(connection, transaction, tech);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(TechModel).Name));
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
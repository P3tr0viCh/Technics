using P3tr0viCh.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Technics.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        private async Task TechDeleteAsync(
            DbConnection connection, DbTransaction transaction, TechModel tech)
        {
            await connection.ExecuteSqlAsync(ResourcesSql.ClearMileagesMileageCommonByTechId,
                new { techid = tech.Id }, transaction);

            await connection.ExecuteSqlAsync(ResourcesSql.ClearTechPartsMileagesByTechId,
                new { techid = tech.Id }, transaction);

            await connection.ListItemDeleteAsync(tech, transaction);
        }

        public async Task TechDeleteAsync(IEnumerable<TechModel> techs)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var tech in techs)
                        {
                            await TechDeleteAsync(connection, transaction, tech);
                        }

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(techs);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task TechDeleteAsync(TechModel tech)
        {
            await TechDeleteAsync(new List<TechModel>() { tech });
        }
    }
}
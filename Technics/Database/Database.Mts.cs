using P3tr0viCh.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        private async Task MtDeleteAsync(DbConnection connection, DbTransaction transaction, MtModel mt)
        {
            await connection.ExecuteSqlAsync(ResourcesSql.ClearMaintenanceMileagesByMtId,
                new { mtid = mt.Id }, transaction);

            await connection.ListItemDeleteAsync(mt, transaction);
        }

        public async Task MtDeleteAsync(IEnumerable<MtModel> mts)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var mt in mts)
                        {
                            await MtDeleteAsync(connection, transaction, mt);
                        }

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(mts);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task MtDeleteAsync(MtModel mt)
        {
            await MtDeleteAsync(new List<MtModel>() { mt });
        }
    }
}
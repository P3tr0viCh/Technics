using P3tr0viCh.Database;
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
        private async Task PartDeleteAsync(
           DbConnection connection, DbTransaction transaction, PartModel part)
        {
            await connection.ExecuteSqlAsync(ResourcesSql.ClearTechPartsMileagesByPartId,
                new { partid = part.Id }, transaction);

            await connection.ListItemDeleteAsync(part, transaction);
        }

        public async Task PartDeleteAsync(IEnumerable<PartModel> parts)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var part in parts)
                        {
                            await PartDeleteAsync(connection, transaction, part);
                        }

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(parts);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task PartDeleteAsync(PartModel part)
        {
            await PartDeleteAsync(new List<PartModel>() { part });
        }
    }
}
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        public async Task<double?> MileagesGetMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var query = new Query()
            {
                Fields = "SUM(mileage)",
                Table = Tables.mileages,
                Where = "techid = :techid AND datetime <= :datetime"
            };

            object param = new { techid = mileage.TechId, datetime = mileage.DateTime };

            return await Actions.QueryFirstOrDefaultAsync<double?>(connection, query, param, transaction);
        }

        public async Task<double> MileagesGetMileageCommonPrevAsync(MileageModel mileage)
        {
            var query = new Query()
            {
                Fields = "IFNULL(SUM(mileage), 0.0)",
                Table = Tables.mileages,
                Where = "id != :id AND techid = :techid AND datetime <= :datetime"
            };

            object param = new { id = mileage.Id, techid = mileage.TechId, datetime = mileage.DateTime };

            using (var connection = GetConnection())
            {
                return await Actions.QueryFirstOrDefaultAsync<double>(connection, query, param);
            }
        }

        private async Task MileagesUpdateMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            await Actions.ExecuteAsync(connection,
                ResourcesSql.UpdateMileagesMileageCommonById,
                new { id = mileage.Id, mileagecommon = mileage.MileageCommon }, transaction);
        }

        private Query MileagesGetChangedQuery()
        {
            return new Query()
            {
                Fields = "id, techid, datetime",
                Table = Tables.mileages,
            };
        }

        private async Task<MileageModel> MileagesGetChangedCurrentAsync(
            DbConnection connection, DbTransaction transaction, long id)
        {
            var query = MileagesGetChangedQuery();

            query.Where = $"id = :id";

            object param = new { id };

            return await Actions.QueryFirstOrDefaultAsync<MileageModel>(connection, query, param, transaction);
        }

        private async Task<IEnumerable<MileageModel>> MileagesGetUpdatedAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            if (mileage.TechId == null) return Enumerable.Empty<MileageModel>();

            var query = MileagesGetChangedQuery();

            query.Where = $"techid = :techid AND datetime > :datetime";

            object param = new
            {
                techid = mileage.TechId,
                datetime = mileage.DateTime,
            };

            return await Actions.ListLoadAsync<MileageModel>(connection, query, param, transaction);
        }

        private async Task MileagesGetChangedAsync(
            DbConnection connection, DbTransaction transaction, ChangedMileageModel changed, MileageModel mileage)
        {
            changed.ChangedMileages.Add(mileage);

            var updated = await MileagesGetUpdatedAsync(connection, transaction, mileage);

            changed.UpdatedMileages.AddRange(updated);
        }

        private async Task<ChangedMileageModel> MileagesGetChangedAfterDeleteAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var changed = new ChangedMileageModel();

            await MileagesGetChangedAsync(connection, transaction, changed, mileage);

            return changed;
        }

        private async Task<ChangedMileageModel> MileagesGetChangedAfterChangeAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var changed = new ChangedMileageModel();

            if (mileage.IsNew)
            {
                await MileagesGetChangedAsync(connection, transaction, changed, mileage);

                return changed;
            }

            var currentMileage = await MileagesGetChangedCurrentAsync(connection, transaction, mileage.Id);

            if (currentMileage.TechId != mileage.TechId ||
                currentMileage.DateTime != mileage.DateTime)
            {
                await MileagesGetChangedAsync(connection, transaction, changed, currentMileage);
            }

            await MileagesGetChangedAsync(connection, transaction, changed, mileage);

            return changed;
        }

        private async Task<List<UpdateMileageModel>> MileagesUpdateMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<MileageModel> mileages)
        {
            var updated = new List<UpdateMileageModel>();

            DebugWrite.Line($"count={mileages.Count()}");

            foreach (var mileage in mileages)
            {
                mileage.MileageCommon = await MileagesGetMileageCommonAsync(connection, transaction, mileage);

                await MileagesUpdateMileageCommonAsync(connection, transaction, mileage);

                updated.Add(new UpdateMileageModel()
                {
                    Id = mileage.Id,
                    MileageCommon = mileage.MileageCommon,
                });
            }

            return updated;
        }

        private async Task<UpdateModel> MileageSaveAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var update = new UpdateModel();

            var changedMileages = await MileagesGetChangedAfterChangeAsync(connection, transaction, mileage);

            await Actions.ListItemSaveAsync(connection, mileage, transaction);

            update.Mileages = await MileagesUpdateMileageCommonAsync(connection, transaction, changedMileages.UpdatedMileages);

            var changedTechParts = await TechPartsGetChangedAfterChangeMileagesAsync(connection, transaction, changedMileages.ChangedMileages);

            update.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, changedTechParts);

            return update;
        }

        public async Task<UpdateModel> MileageSaveAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var changes = await MileageSaveAsync(connection, transaction, mileage);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(MileageModel).Name));

                        return changes;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task<UpdateModel> MileageDeleteAsync(
            DbConnection connection, DbTransaction transaction, MileageModel mileage)
        {
            var update = new UpdateModel();

            var changedMileages = await MileagesGetChangedAfterDeleteAsync(connection, transaction, mileage);

            await Actions.ListItemDeleteAsync(connection, mileage, transaction);

            update.Mileages = await MileagesUpdateMileageCommonAsync(connection, transaction, changedMileages.UpdatedMileages);

            var changedTechParts = await TechPartsGetChangedAfterChangeMileagesAsync(connection, transaction, changedMileages.ChangedMileages);

            update.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, changedTechParts);

            return update;
        }

        public async Task<UpdateModel> MileageDeleteAsync(MileageModel mileage)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var changes = await MileageDeleteAsync(connection, transaction, mileage);

                        transaction.Commit();

                        Utils.Log.Info(string.Format(ResourcesLog.ListItemDeleteOk, typeof(MileageModel).Name));

                        return changes;
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
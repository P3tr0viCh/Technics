using Newtonsoft.Json;
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

        private async Task<List<UpdateMileageModel>> MileagesUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, long? techId)
        {
            var updated = new List<UpdateMileageModel>();

            if (techId == null || techId == Sql.NewId) return updated;

            var query = new Query()
            {
                Table = Tables.mileages,
                Where = "techid = :techid",
                Order = "datetime"
            };

            object param = new { techid = techId };

            var mileages = await Actions.ListLoadAsync<MileageModel>(connection, query, param, transaction);

            if (!mileages.Any()) return updated;

            var mileageCommon = 0.0;

            foreach (var mileage in mileages)
            {
                mileageCommon += mileage.Mileage;

                if (mileage.MileageCommon == mileageCommon) continue;

                mileage.MileageCommon = mileageCommon;

                await Actions.ExecuteAsync(connection,
                    ResourcesSql.UpdateMileagesMileageCommonById,
                    new { id = mileage.Id, mileagecommon = mileage.MileageCommon }, transaction);

                updated.Add(new UpdateMileageModel() { Id = mileage.Id, MileageCommon = mileageCommon });
            }

            return updated;
        }

        private async Task<List<UpdateMileageModel>> MileagesUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long?> techs)
        {
            var updated = new List<UpdateMileageModel>();

            DebugWrite.Line($"techs: {JsonConvert.SerializeObject(techs)}");

            foreach (var tech in techs)
            {
                var changed = await MileagesUpdateMileagesAsync(connection, transaction, tech);

                updated.AddRange(changed);
            }

            if (updated.Count > 0)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(MileageModel).Name, updated.Count));
            }

            return updated;
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
                        var update = new UpdateModel();

                        var techs = new List<long?>() { mileage.TechId };

                        if (!mileage.IsNew)
                        {
                            var prevValue = await ListItemLoadByIdAsync<MileageModel>(connection, transaction, mileage.Id);

                            if (prevValue?.TechId != mileage.TechId)
                            {
                                techs.Add(prevValue.TechId);
                            }
                        }

                        await Actions.ListItemSaveAsync(connection, mileage, transaction);

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techs);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk<MileageModel>();

                        return update;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<UpdateModel> MileageSaveAsync(IEnumerable<MileageModel> mileages)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var update = new UpdateModel();

                        await Actions.ListItemSaveAsync(connection, mileages, transaction);

                        var techs = mileages.Select(m => m.TechId).Distinct();

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techs);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk(mileages);

                        return update;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<UpdateModel> MileageDeleteAsync(IEnumerable<MileageModel> mileages)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var update = new UpdateModel();

                        await Actions.ListItemDeleteAsync(connection, mileages, transaction);

                        var techs = mileages.Select(m => m.TechId).Distinct();

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techs);

                        //            var changedTechParts = await TechPartsGetChangedAfterChangeMileagesAsync(connection, transaction, changedMileages.ChangedMileages);

                        //            update.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, changedTechParts);

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(mileages);

                        return update;
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
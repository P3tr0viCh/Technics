using Newtonsoft.Json;
using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Filter;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        public async Task<IEnumerable<MileageModel>> MileagesLoadAsync(Mileages filter)
        {
            var query = new Query
            {
                Sql = ResourcesSql.SelectMileages,
                Where = filter.ToString(),
                Order = "datetime DESC"
            };

            return await ListLoadAsync<MileageModel>(query);
        }

        public async Task<IEnumerable<MileageModel>> MileagesLoadAsync(IEnumerable<TechModel> techs)
        {
            var filter = new Mileages()
            {
                Techs = techs
            };

            return await MileagesLoadAsync(filter);
        }

        public async Task<double> MileagesGetMileageCommonPrevAsync(MileageModel mileage)
        {
            var query = new Query()
            {
                Fields = "mileagecommon",
                Table = Tables.mileages,
                Where = "techid = :techid AND datetime < :datetime",
                Order = "datetime DESC"
            };

            object param = new { id = mileage.Id, techid = mileage.TechId, datetime = mileage.DateTime };

            using (var connection = GetConnection())
            {
                return await connection.QuerySingleRowAsync<double>(query, param);
            }
        }

        public async Task<double> MileagesGetMileageCommonNextAsync(MileageModel mileage)
        {
            var query = new Query()
            {
                Fields = "mileagecommon",
                Table = Tables.mileages,
                Where = "techid = :techid AND datetime > :datetime",
                Order = "datetime"
            };

            object param = new { id = mileage.Id, techid = mileage.TechId, datetime = mileage.DateTime };

            using (var connection = GetConnection())
            {
                return await connection.QuerySingleRowAsync<double>(query, param);
            }
        }

        private async Task<List<UpdateMileageModel>> MileagesUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, long techId)
        {
            var updated = new List<UpdateMileageModel>();

            if (techId == Sql.NewId) return updated;

            var query = new Query()
            {
                Table = Tables.mileages,
                Where = "techid = :techid",
                Order = "datetime"
            };

            object param = new { techid = techId };

            var mileages = await connection.ListLoadAsync<MileageModel>(query, param, transaction);

            if (!mileages.Any()) return updated;

            double mileageValue;

            var mileageCommonValue = 0.0;

            foreach (var mileage in mileages)
            {
                if (mileage.MileageType == MileageType.Single)
                {
                    mileageValue = mileage.Mileage;

                    mileageCommonValue += mileage.Mileage;
                }
                else
                {
                    mileageValue = (double)mileage.MileageCommon - mileageCommonValue;

                    mileageCommonValue = (double)mileage.MileageCommon;
                }

                if (mileage.Mileage == mileageValue &&
                    mileage.MileageCommon == mileageCommonValue) continue;

                mileage.Mileage = mileageValue;

                mileage.MileageCommon = mileageCommonValue;

                await connection.ExecuteSqlAsync(ResourcesSql.UpdateMileagesMileagesById,
                    new
                    {
                        id = mileage.Id,
                        mileage = mileage.Mileage,
                        mileagecommon = mileage.MileageCommon
                    }, transaction);

                updated.Add(new UpdateMileageModel()
                {
                    Id = mileage.Id,
                    Mileage = mileageValue,
                    MileageCommon = mileageCommonValue
                });
            }

            return updated;
        }

        private async Task<List<UpdateMileageModel>> MileagesUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long> techIds)
        {
            var updated = new List<UpdateMileageModel>();

            DebugWrite.Line($"techs: {JsonConvert.SerializeObject(techIds)}");

            foreach (var techId in techIds)
            {
                var changed = await MileagesUpdateMileagesAsync(connection, transaction, techId);

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

                        var techIds = new List<long>();

                        Utils.ListAddNotNull(techIds, mileage.TechId);

                        if (!mileage.IsNew)
                        {
                            var prevValue = await connection.ListItemLoadByIdAsync<MileageModel>(transaction, mileage.Id);

                            if (prevValue?.TechId != mileage.TechId)
                            {
                                Utils.ListAddNotNull(techIds, prevValue.TechId);
                            }
                        }

                        await connection.ListItemSaveAsync(mileage, transaction);

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techIds);

                        update.TechParts = await TechPartsUpdateMileagesForTechsAsync(connection, transaction, techIds);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk(mileage);

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

                        await connection.ListItemSaveAsync(mileages, transaction);

                        var techIds = mileages.Select(mileage => mileage.TechId).DistinctNotNullLong();

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techIds);

                        update.TechParts = await TechPartsUpdateMileagesForTechsAsync(connection, transaction, techIds);

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

                        await connection.ListItemDeleteAsync(mileages, transaction);

                        var techIds = mileages.Select(mileage => mileage.TechId).Distinct()
                            .Where(id => id != null).Cast<long>();

                        update.Mileages = await MileagesUpdateMileagesAsync(connection, transaction, techIds);

                        update.TechParts = await TechPartsUpdateMileagesForTechsAsync(connection, transaction, techIds);

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
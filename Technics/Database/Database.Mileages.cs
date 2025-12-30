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
        public async Task<IEnumerable<MileageModel>> MileagesLoadAsync(Filter.Mileages filter)
        {
            var where = filter.ToString();

            var sql = string.Format(ResourcesSql.SelectMileages, where);

            return await ListLoadAsync<MileageModel>(sql);
        }

        public async Task<IEnumerable<MileageModel>> MileagesLoadAsync(IEnumerable<TechModel> techs)
        {
            var filter = new Filter.Mileages()
            {
                Techs = techs
            };

            return await MileagesLoadAsync(filter);
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
                return await connection.QuerySingleRowAsync<double>(query, param);
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

            var mileages = await connection.ListLoadAsync<MileageModel>(query, param, transaction);

            if (!mileages.Any()) return updated;

            var mileageCommon = 0.0;

            foreach (var mileage in mileages)
            {
                mileageCommon += mileage.Mileage;

                if (mileage.MileageCommon == mileageCommon) continue;

                mileage.MileageCommon = mileageCommon;

                await connection.ExecuteSqlAsync(ResourcesSql.UpdateMileagesMileageCommonById,
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
                            var prevValue = await connection.ListItemLoadByIdAsync<MileageModel>(transaction, mileage.Id);

                            if (prevValue?.TechId != mileage.TechId)
                            {
                                techs.Add(prevValue.TechId);
                            }
                        }

                        await connection.ListItemSaveAsync(mileage, transaction);

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

                        await connection.ListItemSaveAsync(mileages, transaction);

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

                        await connection.ListItemDeleteAsync(mileages, transaction);

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
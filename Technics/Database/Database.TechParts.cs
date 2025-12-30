using Newtonsoft.Json;
using P3tr0viCh.Database;
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
        public async Task<IEnumerable<TechPartModel>> TechPartsLoadAsync(TechParts filter)
        {
            var query = new Query
            {
                Sql = ResourcesSql.SelectTechParts,
                Where = filter.ToString(),
                Order = "datetimeinstall DESC"
            };

            return await ListLoadAsync<TechPartModel>(query);
        }

        public async Task<IEnumerable<TechPartModel>> TechPartsLoadAsync(IEnumerable<TechModel> techs)
        {
            var filter = new TechParts()
            {
                Techs = techs
            };

            return await TechPartsLoadAsync(filter);
        }

        private async Task<double?> TechPartsGetMileageAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            var query = new Query()
            {
                Fields = "SUM(mileage)",
                Table = Tables.mileages
            };

            if (techPart.DateTimeRemove == null)
            {
                query.Where = "techid = :techid AND datetime >= :datetimeinstall";
            }
            else
            {
                query.Where = "techid = :techid AND datetime >= :datetimeinstall AND datetime < :datetimeremove";
            }

            object param = new
            {
                techid = techPart.TechId,
                datetimeinstall = techPart.DateTimeInstall,
                datetimeremove = techPart.DateTimeRemove
            };

            return await connection.QuerySingleRowAsync<double?>(query, param, transaction);
        }

        private async Task TechPartsUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            if (techPart.Mileage == null)
            {
                techPart.MileageCommon = null;
            }

            await connection.ExecuteSqlAsync(ResourcesSql.UpdateTechPartsMileagesById,
                            new
                            {
                                id = techPart.Id,
                                mileage = techPart.Mileage,
                                mileagecommon = techPart.MileageCommon
                            },
                        transaction);
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForPartAsync(
            DbConnection connection, DbTransaction transaction, long? partId)
        {
            var updated = new List<UpdateTechPartModel>();

            if (partId == null || partId == Sql.NewId) return updated;

            var query = new Query()
            {
                Table = Tables.techparts,
                Where = "partid = :partid",
                Order = "datetimeinstall"
            };

            object param = new { partid = partId };

            var techParts = await connection.ListLoadAsync<TechPartModel>(query, param, transaction);

            if (!techParts.Any()) return updated;

            var mileageCommon = 0.0;

            foreach (var techPart in techParts)
            {
                var mileage = await TechPartsGetMileageAsync(connection, transaction, techPart);

                if (mileage != null)
                {
                    mileageCommon += (double)mileage;
                }

                if (techPart.Mileage == mileage &&
                    techPart.MileageCommon == mileageCommon) continue;

                techPart.Mileage = mileage;
                techPart.MileageCommon = mileageCommon;

                await TechPartsUpdateMileagesAsync(connection, transaction, techPart);

                updated.Add(new UpdateTechPartModel()
                {
                    Id = techPart.Id,
                    Mileage = techPart.Mileage,
                    MileageCommon = techPart.MileageCommon
                });
            }

            return updated;
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForPartsAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long?> partIds)
        {
            DebugWrite.Line($"partIds: {JsonConvert.SerializeObject(partIds)}");

            var updated = new List<UpdateTechPartModel>();

            foreach (var partId in partIds)
            {
                var changed = await TechPartsUpdateMileagesForPartAsync(connection, transaction, partId);

                updated.AddRange(changed);
            }

            if (updated.Count > 0)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(TechParts).Name, updated.Count));
            }

            return updated;
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForTechsAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long> techIds)
        {
            DebugWrite.Line($"techIds: {JsonConvert.SerializeObject(techIds)}");

            var query = new Query()
            {
                Fields = "id, partid",
                Table = Tables.techparts,
                Where = ByIdToString("techid", techIds)
            };

            var techParts = await connection.ListLoadAsync<TechPartModel>(query, null, transaction);

            var parts = techParts.Select(techPart => techPart.PartId).Distinct();

            var updated = await TechPartsUpdateMileagesForPartsAsync(connection, transaction, parts);

            return updated;
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForTechsOrPartAsync(
            DbConnection connection, DbTransaction transaction,
            IEnumerable<long> techIds, IEnumerable<long> partIds)
        {
            DebugWrite.Line($"techIds: {JsonConvert.SerializeObject(techIds)}, partIds: {JsonConvert.SerializeObject(partIds)}");

            var query = new Query()
            {
                Fields = "id, partid",
                Table = Tables.techparts,
            };

            var where = ByIdToString("techid", techIds);

            where = where.JoinExcludeEmpty(" OR ", ByIdToString("partid", partIds));

            query.Where = where;

            var techParts = await connection.ListLoadAsync<TechPartModel>(query, null, transaction);

            var parts = techParts.Select(techPart => techPart.PartId).Distinct();

            var updated = await TechPartsUpdateMileagesForPartsAsync(connection, transaction, parts);

            return updated;
        }

        public async Task<UpdateModel> TechPartSaveAsync(TechPartModel techPart)
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

                        var partIds = new List<long>();

                        Utils.ListAddNotNull(techIds, techPart.TechId);
                        Utils.ListAddNotNull(techIds, techPart.PartId);

                        if (!techPart.IsNew)
                        {
                            var prevValue = await connection.ListItemLoadByIdAsync<TechPartModel>(transaction, techPart.Id);

                            if (prevValue?.TechId != techPart.TechId)
                            {
                                Utils.ListAddNotNull(techIds, prevValue.TechId);
                            }

                            if (prevValue?.PartId != techPart.PartId)
                            {
                                Utils.ListAddNotNull(partIds, prevValue.PartId);
                            }
                        }

                        await connection.ListItemSaveAsync(techPart, transaction);

                        update.TechParts = await TechPartsUpdateMileagesForTechsOrPartAsync(
                            connection, transaction, techIds, partIds);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk<TechPartModel>();

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

        public async Task<UpdateModel> TechPartDeleteAsync(IEnumerable<TechPartModel> techParts)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var update = new UpdateModel();

                        await connection.ListItemDeleteAsync(techParts, transaction);

                        var techIds = techParts.Select(techPart => techPart.TechId).DistinctNotNullLong();

                        var partIds = techParts.Select(techPart => techPart.PartId).DistinctNotNullLong();

                        update.TechParts = await TechPartsUpdateMileagesForTechsOrPartAsync(
                            connection, transaction, techIds, partIds);

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(techParts);

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
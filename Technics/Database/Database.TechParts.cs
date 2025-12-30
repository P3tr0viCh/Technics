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

        private async Task<double?> TechPartsGetMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            var query = new Query()
            {
                Fields = "id, techid, datetimeinstall, datetimeremove",
                Table = Tables.techparts,
                Where = "partid = :partid AND datetimeinstall <= :datetimeinstall",
                Order = "datetimeinstall DESC"
            };

            object param = new
            {
                partid = techPart.PartId,
                datetimeinstall = techPart.DateTimeInstall,
                datetimeremove = techPart.DateTimeRemove
            };

            var parts = await connection.ListLoadAsync<TechPartModel>(query, param, transaction);

            DebugWrite.Line($"{parts.Count()}");

            var mileageCommon = 0.0;

            foreach (var part in parts)
            {
                var mileage = await TechPartsGetMileageAsync(connection, transaction, part);

                if (mileage == null) continue;

                mileageCommon += (double)mileage;
            }

            if (mileageCommon != 0) return mileageCommon;

            return null;
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

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForTechsAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long?> techIds)
        {
            var updated = new List<UpdateTechPartModel>();

            DebugWrite.Line($"techs: {JsonConvert.SerializeObject(techIds)}");

            var query = new Query()
            {
                Fields = "id, partid",
                Table = Tables.techparts,
            };

            var techs = new List<TechModel>();

            foreach (var techId in techIds)
            {
                if (techId == null) continue;

                techs.Add(new TechModel() { Id = (long)techId });
            }

            var filter = new TechParts()
            {
                Techs = techs
            };

            query.Where = filter.ToString();

            var techParts = await connection.ListLoadAsync<TechPartModel>(query, null, transaction);

            var parts = techParts.Select(techPart => techPart.PartId).Distinct();

            foreach (var part in parts)
            {
                var changed = await TechPartsUpdateMileagesForPartAsync(connection, transaction, part);

                updated.AddRange(changed);
            }

            if (updated.Count > 0)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(TechParts).Name, updated.Count));
            }

            return updated;
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesForTechsOrPartAsync(
            DbConnection connection, DbTransaction transaction,
            IEnumerable<long?> techIds, IEnumerable<long?> partIds)
        {
            var updated = new List<UpdateTechPartModel>();

            DebugWrite.Line($"techs: {JsonConvert.SerializeObject(techIds)}");

            var query = new Query()
            {
               Table = Tables.techparts,
            };

            var where = ByIdToString("techid", techIds);

            where = where.JoinExcludeEmpty(" OR ", ByIdToString("partid", partIds));

            query.Where = where;

            var techParts = await connection.ListLoadAsync<TechPartModel>(query, null, transaction);

            var changed = await TechPartsUpdateMileagesAsync(connection, transaction, techParts);

            updated.AddRange(changed);

            if (updated.Count > 0)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(TechParts).Name, updated.Count));
            }

            return updated;
        }

        private Query TechPartsGetChangedQuery()
        {
            return new Query()
            {
                Table = Tables.techparts,
            };
        }

        private async Task<TechPartModel> TechPartsGetChangedCurrentAsync(
            DbConnection connection, DbTransaction transaction, long id)
        {
            var query = TechPartsGetChangedQuery();

            query.Where = $"id = :id";

            object param = new { id };

            return await connection.QuerySingleRowAsync<TechPartModel>(query, param, transaction);
        }

        private async Task<IEnumerable<TechPartModel>> TechPartsGetUpdatedAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            var query = TechPartsGetChangedQuery();

            query.Where = $"id != :id AND partid = :partid AND datetimeinstall > :datetimeinstall";

            object param = new
            {
                id = techPart.Id,
                partid = techPart.PartId,
                datetimeinstall = techPart.DateTimeInstall,
            };

            return await connection.ListLoadAsync<TechPartModel>(query, param, transaction);
        }

        private async Task TechPartsGetChangedAsync(
            DbConnection connection, DbTransaction transaction, ChangedTechPartModel changed, TechPartModel techPart)
        {
            changed.ChangedTechParts.Add(techPart);

            var updated = await TechPartsGetUpdatedAsync(connection, transaction, techPart);

            changed.UpdatedTechParts.AddRange(updated);
        }

        private async Task<List<UpdateTechPartModel>> TechPartsUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<TechPartModel> techParts)
        {
            var updated = new List<UpdateTechPartModel>();

            DebugWrite.Line($"count={techParts.Count()}");

            foreach (var techPart in techParts)
            {
                techPart.Mileage = await TechPartsGetMileageAsync(connection, transaction, techPart);
                techPart.MileageCommon = await TechPartsGetMileageCommonAsync(connection, transaction, techPart);

                await TechPartsUpdateMileagesAsync(connection, transaction, techPart);

                updated.Add(new UpdateTechPartModel()
                {
                    Id = techPart.Id,
                    Mileage = techPart.Mileage,
                    MileageCommon = techPart.MileageCommon,
                });
            }

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

                        var techIds = new List<long?>() { techPart.TechId };

                        var partIds = new List<long?>() { techPart.PartId };

                        if (!techPart.IsNew)
                        {
                            var prevValue = await connection.ListItemLoadByIdAsync<TechPartModel>(transaction, techPart.Id);

                            if (prevValue?.TechId != techPart.TechId)
                            {
                                techIds.Add(prevValue.TechId);
                            }

                            if (prevValue?.PartId != techPart.PartId)
                            {
                                partIds.Add(prevValue.PartId);
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

                        var techIds = techParts.Select(techPart => techPart.TechId).Distinct();

                        var partIds = techParts.Select(techPart => techPart.PartId).Distinct();

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
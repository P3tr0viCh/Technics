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

            return await Actions.QueryFirstOrDefaultAsync<double?>(connection, query, param, transaction);
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

            var parts = await Actions.ListLoadAsync<TechPartModel>(connection, query, param, transaction);

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
            if (techPart.Mileage ==  null)
            {
                techPart.MileageCommon = null;
            }

            await Actions.ExecuteAsync(connection,
                        ResourcesSql.UpdateTechPartsMileagesById,
                            new
                            {
                                id = techPart.Id,
                                mileage = techPart.Mileage,
                                mileagecommon = techPart.MileageCommon
                            },
                        transaction);
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

            return await Actions.QueryFirstOrDefaultAsync<TechPartModel>(connection, query, param, transaction);
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

            return await Actions.ListLoadAsync<TechPartModel>(connection, query, param, transaction);
        }

        private async Task TechPartsGetChangedAsync(
            DbConnection connection, DbTransaction transaction, ChangedTechPartModel changed, TechPartModel techPart)
        {
            changed.ChangedTechParts.Add(techPart);

            var updated = await TechPartsGetUpdatedAsync(connection, transaction, techPart);

            changed.UpdatedTechParts.AddRange(updated);
        }

        private async Task<ChangedTechPartModel> TechPartsGetChangedAfterChangeTechPartAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            var changed = new ChangedTechPartModel();

            if (techPart.IsNew)
            {
                await TechPartsGetChangedAsync(connection, transaction, changed, techPart);

                return changed;
            }

            var currentTechPart = await TechPartsGetChangedCurrentAsync(connection, transaction, techPart.Id);

            if (currentTechPart.TechId != techPart.TechId ||
                currentTechPart.PartId != techPart.PartId ||
                currentTechPart.DateTimeInstall != techPart.DateTimeInstall ||
                currentTechPart.DateTimeRemove != techPart.DateTimeRemove)
            {
                await TechPartsGetChangedAsync(connection, transaction, changed, currentTechPart);
            }

            await TechPartsGetChangedAsync(connection, transaction, changed, techPart);

            return changed;
        }

        private async Task<IEnumerable<TechPartModel>> TechPartsGetChangedAfterChangeMileagesAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<MileageModel> mileages)
        {
            var techsChanged = mileages.Select(m => (long)m.TechId);

            if (!techsChanged.Any()) return Enumerable.Empty<TechPartModel>();

            var datetime = mileages.Select(m => m.DateTime).Min();

            DebugWrite.Line(JsonConvert.SerializeObject(techsChanged));

            var query = new Query()
            {
                Fields = "id, techid, partid, datetimeinstall, datetimeremove, mileage",
                Table = Tables.techparts,
                Where = $"{Filter.ByIdToString("techid", techsChanged)}"
            };

            return await Actions.ListLoadAsync<TechPartModel>(connection, query, null, transaction);
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

        private async Task<UpdateModel> TechPartSaveAsync(
            DbConnection connection, DbTransaction transaction, TechPartModel techPart)
        {
            var update = new UpdateModel();

            var changedTechParts = await TechPartsGetChangedAfterChangeTechPartAsync(connection, transaction, techPart);

            techPart.Mileage = await TechPartsGetMileageAsync(connection, transaction, techPart);
            techPart.MileageCommon = await TechPartsGetMileageCommonAsync(connection, transaction, techPart);

            DebugWrite.Line(JsonConvert.SerializeObject(techPart));

            await Actions.ListItemSaveAsync(connection, techPart, transaction);

            update.TechParts = await TechPartsUpdateMileagesAsync(connection, transaction, changedTechParts.UpdatedTechParts);

            return update;
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
                        var changes = await TechPartSaveAsync(connection, transaction, techPart);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk<TechPartModel>();

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
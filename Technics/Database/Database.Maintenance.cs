using Newtonsoft.Json;
using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
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
        public async Task<IEnumerable<MaintenanceModel>> MaintenanceLoadAsync(Maintenance filter)
        {
            var query = new Query
            {
                Sql = ResourcesSql.SelectMaintenance,
                Where = filter.ToString(),
                Order = "datetime DESC"
            };

            return await ListLoadAsync<MaintenanceModel>(query);
        }

        public async Task<IEnumerable<MaintenanceModel>> MaintenanceLoadAsync(IEnumerable<TechModel> techs)
        {
            if (techs.IsEmpty())
            {
                return Enumerable.Empty<MaintenanceModel>();
            }

            var filter = new Maintenance()
            {
                Techs = techs
            };

            return await MaintenanceLoadAsync(filter);
        }

        private async Task<double?> MaintenanceGetMileageCommonAsync(
            DbConnection connection, DbTransaction transaction, MaintenanceModel maintenance)
        {
            var mileage = new MileageModel()
            {
                TechId = maintenance.TechId,
                DateTime = maintenance.DateTime,
            };

            return await MileagesGetMileageCommonPrevAsync(mileage);
        }

        private async Task<double?> MaintenanceGetMileageAfterMaintenanceCommonAsync(
                   DbConnection connection, DbTransaction transaction, MaintenanceModel maintenance)
        {
            var query = new Query
            {
                Fields = "id, datetime, mileagecommon",
                Table = Tables.maintenance,
                Where = "techid = @techid AND mtid = @mtid AND datetime > @datetime",
                Order = "datetime",
                Limit = 1
            };

            object param = new
            {
                techid = maintenance.TechId,
                mtid = maintenance.MtId,
                datetime = maintenance.DateTime,
            };

            var next = await connection.QuerySingleRowAsync<MaintenanceModel>(query, param, transaction);

            if (next != null) return next.MileageCommon - maintenance.MileageCommon;

            query = new Query
            {
                Fields = "mileagecommon",
                Table = Tables.mileages,
                Where = "techid = @techid",
                Order = "datetime DESC",
                Limit = 1
            };

            param = new
            {
                techid = maintenance.TechId,
            };

            var last = await connection.QuerySingleRowAsync<double>(query, param, transaction);

            if (last == 0) return 0.0;

            return last - maintenance.MileageCommon;
        }

        private async Task MaintenanceUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, MaintenanceModel maintenance)
        {
            if (maintenance.MileageCommon == 0.0)
            {
                maintenance.MileageCommon = null;
            }

            if (maintenance.MileageAfterMaintenance == 0.0)
            {
                maintenance.MileageAfterMaintenance = null;
            }

            await connection.ExecuteSqlAsync(ResourcesSql.UpdateMaintenanceMileagesById,
                            new
                            {
                                id = maintenance.Id,
                                mileagecommon = maintenance.MileageCommon,
                                mileageaftermaintenance = maintenance.MileageAfterMaintenance,
                            },
                        transaction);
        }

        private async Task<List<UpdateMaintenanceModel>> MaintenanceUpdateMileagesByIdAsync(
            DbConnection connection, DbTransaction transaction, long id)
        {
            var updated = new List<UpdateMaintenanceModel>();

            if (id == Sql.NewId) return updated;

            var maintenance = await connection.ListItemLoadByIdAsync<MaintenanceModel>(id, transaction);

            if (maintenance == null) return updated;

            var changed = false;

            var mileageCommon =
                await MaintenanceGetMileageCommonAsync(connection, transaction, maintenance);

            if (maintenance.MileageCommon != mileageCommon)
            {
                changed = true;

                maintenance.MileageCommon = mileageCommon;
            }

            var mileageAfterMaintenance = 
                await MaintenanceGetMileageAfterMaintenanceCommonAsync(connection, transaction, maintenance);

            if (maintenance.MileageAfterMaintenance != mileageAfterMaintenance)
            {
                changed = true;

                maintenance.MileageAfterMaintenance = mileageAfterMaintenance;
            }

            if (!changed) return updated;

            await MaintenanceUpdateMileagesAsync(connection, transaction, maintenance);

            updated.Add(new UpdateMaintenanceModel()
            {
                Id = maintenance.Id,
                MileageCommon = maintenance.MileageCommon,
                MileageAfterMaintenance = maintenance.MileageAfterMaintenance,
            });

            return updated;
        }

        private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesForTechsAsync(
             DbConnection connection, DbTransaction transaction, IEnumerable<long> techIds)
        {
            DebugWrite.Line($"techIds: {JsonConvert.SerializeObject(techIds)}");

            var query = new Query()
            {
                Fields = "id",
                Table = Tables.maintenance,
                Where = ByIdToString("techid", techIds),
                Order = "datetime DESC",
            };

            var maintenances = await connection.ListLoadAsync<MaintenanceModel>(query, null, transaction);

            var maintenanceIds = maintenances.Select(maintenance => maintenance.Id).Distinct();

            var updated = await MaintenancesUpdateMileagesAsync(connection, transaction, maintenanceIds);

            return updated;
        }

        private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesAsync(
            DbConnection connection, DbTransaction transaction, IEnumerable<long> maintenanceIds)
        {
            DebugWrite.Line($"maintenanceIds: {JsonConvert.SerializeObject(maintenanceIds)}");

            var updated = new List<UpdateMaintenanceModel>();

            foreach (var maintenanceId in maintenanceIds)
            {
                var changed = await MaintenanceUpdateMileagesByIdAsync(connection, transaction, maintenanceId);

                updated.AddRange(changed);
            }

            if (updated.Count > 0)
            {
                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(Maintenance).Name, updated.Count));
            }

            return updated;
        }

        private async Task<List<UpdateMaintenanceModel>> MaintenanceUpdateMileagesForTechsOrMtsAsync(
            DbConnection connection, DbTransaction transaction,
            IEnumerable<long> techIds, IEnumerable<long> mtIds)
        {
            DebugWrite.Line($"techIds: {JsonConvert.SerializeObject(techIds)}, " +
                $"mtIds: {JsonConvert.SerializeObject(mtIds)}");

            var query = new Query
            {
                Fields = "id",
                Table = Tables.maintenance,
                Where = ByIdToString("techid", techIds).JoinExcludeEmpty(" AND ", ByIdToString("mtid", mtIds)),
                Order = "datetime DESC",
            };

            var maintenances = await connection.ListLoadAsync<MaintenanceModel>(query, null, transaction);

            var maintenanceIds = maintenances.Select(maintenance => maintenance.Id).Distinct();

            var updated = await MaintenancesUpdateMileagesAsync(connection, transaction, maintenanceIds);

            return updated;
        }

        public async Task<UpdateModel> MaintenanceSaveAsync(MaintenanceModel maintenance)
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

                        var mtIds = new List<long>();

                        Utils.ListAddNotNull(techIds, maintenance.TechId);

                        Utils.ListAddNotNull(mtIds, maintenance.MtId);

                        if (!maintenance.IsNew)
                        {
                            var prevValue = await connection.ListItemLoadByIdAsync<MaintenanceModel>(maintenance.Id, transaction);

                            if (prevValue?.TechId != maintenance.TechId)
                            {
                                Utils.ListAddNotNull(techIds, prevValue.TechId);
                            }

                            if (prevValue?.MtId != maintenance.MtId)
                            {
                                Utils.ListAddNotNull(mtIds, prevValue.MtId);
                            }
                        }

                        await connection.ListItemSaveAsync(maintenance, transaction);

                        update.Maintenance = await MaintenanceUpdateMileagesForTechsOrMtsAsync(
                            connection, transaction, techIds, mtIds);

                        transaction.Commit();

                        Utils.Log.ListItemSaveOk(maintenance);

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

        public async Task<UpdateModel> MaintenanceDeleteAsync(IEnumerable<MaintenanceModel> maintenances)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var update = new UpdateModel();

                        await connection.ListItemDeleteAsync(maintenances, transaction);

                        var techIds = maintenances.Select(maintenance => maintenance.TechId).DistinctNotNullLong();

                        var mtIds = maintenances.Select(maintenance => maintenance.MtId).DistinctNotNullLong();

                        update.Maintenance = await MaintenanceUpdateMileagesForTechsOrMtsAsync(
                            connection, transaction, techIds, mtIds);

                        transaction.Commit();

                        Utils.Log.ListItemDeleteOk(maintenances);

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
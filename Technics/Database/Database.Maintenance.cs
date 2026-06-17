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

        /*        private async Task<double?> MaintenancesGetMileageAsync(
                    DbConnection connection, DbTransaction transaction, MaintenanceModel maintenance)
                {
                    var query = new Query()
                    {
                        Fields = "SUM(mileage)",
                        Table = Tables.mileages
                    };

                    if (maintenance.DateTimeRemove == null)
                    {
                        query.Where = "techid = @techid AND datetime >= @datetimeinstall";
                    }
                    else
                    {
                        query.Where = "techid = @techid AND datetime >= @datetimeinstall AND datetime < @datetimeremove";
                    }

                    object param = new
                    {
                        techid = maintenance.TechId,
                        datetimeinstall = maintenance.DateTimeInstall,
                        datetimeremove = maintenance.DateTimeRemove
                    };

                    return await connection.QuerySingleRowAsync<double?>(query, param, transaction);
                }

                private async Task MaintenancesUpdateMileagesAsync(
                    DbConnection connection, DbTransaction transaction, MaintenanceModel maintenance)
                {
                    if (maintenance.MileageCommon == 0.0)
                    {
                        maintenance.MileageCommon = null;
                    }

                    await connection.ExecuteSqlAsync(ResourcesSql.UpdateMaintenancesMileagesById,
                                    new
                                    {
                                        id = maintenance.Id,
                                        mileage = maintenance.Mileage,
                                        mileagecommon = maintenance.MileageCommon
                                    },
                                transaction);
                }

                private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesForPartAsync(
                    DbConnection connection, DbTransaction transaction, long? partId)
                {
                    var updated = new List<UpdateMaintenanceModel>();

                    if (partId == null || partId == Sql.NewId) return updated;

                    var query = new Query()
                    {
                        Table = Tables.techparts,
                        Where = "partid = @partid",
                        Order = "datetimeinstall"
                    };

                    object param = new { partid = partId };

                    var maintenances = await connection.ListLoadAsync<MaintenanceModel>(query, param, transaction);

                    if (!maintenances.Any()) return updated;

                    var mileageCommon = 0.0;

                    foreach (var maintenance in maintenances)
                    {
                        var mileage = await MaintenancesGetMileageAsync(connection, transaction, maintenance);

                        if (mileage != null)
                        {
                            mileageCommon += (double)mileage;
                        }

                        if (maintenance.Mileage == mileage &&
                            maintenance.MileageCommon == mileageCommon) continue;

                        maintenance.Mileage = mileage;
                        maintenance.MileageCommon = mileageCommon;

                        await MaintenancesUpdateMileagesAsync(connection, transaction, maintenance);

                        updated.Add(new UpdateMaintenanceModel()
                        {
                            Id = maintenance.Id,
                            Mileage = maintenance.Mileage,
                            MileageCommon = maintenance.MileageCommon
                        });
                    }

                    return updated;
                }

                private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesForPartsAsync(
                    DbConnection connection, DbTransaction transaction, IEnumerable<long?> partIds)
                {
                    DebugWrite.Line($"partIds: {JsonConvert.SerializeObject(partIds)}");

                    var updated = new List<UpdateMaintenanceModel>();

                    foreach (var partId in partIds)
                    {
                        var changed = await MaintenancesUpdateMileagesForPartAsync(connection, transaction, partId);

                        updated.AddRange(changed);
                    }

                    if (updated.Count > 0)
                    {
                        Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, typeof(Maintenances).Name, updated.Count));
                    }

                    return updated;
                }

                private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesForTechsAsync(
                    DbConnection connection, DbTransaction transaction, IEnumerable<long> techIds)
                {
                    DebugWrite.Line($"techIds: {JsonConvert.SerializeObject(techIds)}");

                    var query = new Query()
                    {
                        Fields = "id, partid",
                        Table = Tables.techparts,
                        Where = ByIdToString("techid", techIds)
                    };

                    var maintenances = await connection.ListLoadAsync<MaintenanceModel>(query, null, transaction);

                    var parts = maintenances.Select(maintenance => maintenance.PartId).Distinct();

                    var updated = await MaintenancesUpdateMileagesForPartsAsync(connection, transaction, parts);

                    return updated;
                }

                private async Task<List<UpdateMaintenanceModel>> MaintenancesUpdateMileagesForTechsOrPartAsync(
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

                    var maintenances = await connection.ListLoadAsync<MaintenanceModel>(query, null, transaction);

                    var parts = maintenances.Select(maintenance => maintenance.PartId).Distinct();

                    var updated = await MaintenancesUpdateMileagesForPartsAsync(connection, transaction, parts);

                    return updated;
                }
        */

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
                        Utils.ListAddNotNull(techIds, maintenance.MtId);

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

                        // update.Maintenances = await MaintenancesUpdateMileagesForTechsOrPartAsync(
                        // connection, transaction, techIds, partIds);

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

                        // update.Maintenances = await MaintenancesUpdateMileagesForTechsOrPartAsync(
                        // connection, transaction, techIds, partIds);

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
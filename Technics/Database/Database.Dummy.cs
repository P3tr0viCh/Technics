using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
#if DEBUG
        public class Dummy
        {
            private const int MileagesCount = 10;

            private const int PartsCount = 5;

            private const int TechPartsCount = 5;


            private readonly Random random = new Random();

            private T GetRandomItem<T>(List<T> list) => list[random.Next(list.Count)];

            private async Task FillTableMileages()
            {
                try
                {
                    await Default.TruncateTableAsync<MileageModel>();

                    var techs = (await Default.ListLoadAsync<TechModel>()).ToList();

                    var mileages = new List<MileageModel>();

                    var mileageCommon = new Dictionary<long, double>();

                    var dateTime = DateTime.Now.AddDays(-MileagesCount);

                    for (var i = 0; i < MileagesCount; i++)
                    {
                        var tech = GetRandomItem(techs);

                        var mileage = new MileageModel()
                        {
                            TechId = tech.Id,
                            TechText = tech.Text,
                            DateTime = dateTime.AddDays(i),
                            Mileage = 10 + random.Next(10),
                        };

                        if (mileageCommon.ContainsKey(tech.Id))
                        {
                            mileageCommon[tech.Id] += mileage.Mileage;
                        }
                        else
                        {
                            mileageCommon[tech.Id] = mileage.Mileage;
                        }

                        mileage.MileageCommon = mileageCommon[tech.Id];

                        mileages.Add(mileage);
                    }

                    await Default.ListItemSaveAsync(mileages);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Utils.Msg.Error(e.Message);
                }
            }

            private async Task FillTableParts()
            {
                try
                {
                    await Default.TruncateTableAsync<PartModel>();

                    var parts = new List<PartModel>();

                    for (var i = 0; i < PartsCount; i++)
                    {
                        var part = new PartModel()
                        {
                            Text = $"Part {random.NextString(5)}"
                        };

                        parts.Add(part);
                    }

                    await Default.ListItemSaveAsync(parts);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Utils.Msg.Error(e.Message);
                }
            }

            private async Task FillTableTechParts()
            {
                try
                {
                    await Default.TruncateTableAsync<TechPartModel>();

                    var techs = (await Default.ListLoadAsync<TechModel>()).ToList();

                    var parts = await Default.ListLoadAsync<PartModel>();

                    var techParts = new List<TechPartModel>();

                    foreach (var part in parts)
                    {
                        var tech = GetRandomItem(techs);

                        var mileages = await Default.ListLoadAsync<MileageModel>(
                            Default.GetMileagesSql(new List<TechModel>() { tech }));

                        if (!mileages.Any()) continue;

                        var dateTimeInstall = mileages.Min(item => item.DateTime);
                        var dateTimeRemove = dateTimeInstall.AddDays(2);

                        for (var i = 0; i < TechPartsCount; i++)
                        {
                            var techPart = new TechPartModel()
                            {
                                TechId = tech.Id,
                                TechText = tech.Text,
                                PartId = part.Id,
                                PartText = part.Text,
                                DateTimeInstall = dateTimeInstall,
                                DateTimeRemove = dateTimeRemove,
                            };

                            techParts.Add(techPart);

                            dateTimeInstall = dateTimeRemove;
                            dateTimeRemove = dateTimeRemove.AddDays(2);
                        }
                    }

                    await Default.ListItemSaveAsync(techParts);

                    using (var connection = Default.GetConnection())
                    {
                        await connection.OpenAsync();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                foreach (var techPart in techParts)
                                {
                                    techPart.Mileage = await Default.TechPartsGetMileageAsync(connection, transaction, techPart);
                                    techPart.MileageCommon = await Default.TechPartsGetMileageCommonAsync(connection, transaction, techPart);

                                    await Default.TechPartsUpdateMileagesAsync(connection, transaction, techPart);
                                }

                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Utils.Msg.Error(e.Message);
                }
            }

            public async Task FillTables()
            {
                await FillTableMileages();

                await FillTableParts();

                await FillTableTechParts();
            }
        }
#endif
    }
}
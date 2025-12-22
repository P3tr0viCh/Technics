using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Technics.Database.Filter;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
#if DEBUG
        public class Dummy
        {
            private const int MileagesCount = 100;

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

            private const int TechPartsCount = 10;

            private async Task FillTableTechParts()
            {
                try
                {
                    await Default.TruncateTableAsync<TechPartModel>();

                    var techs = (await Default.ListLoadAsync<TechModel>()).ToList();

                    var parts = (await Default.ListLoadAsync<PartModel>()).ToList();

                    var techParts = new List<TechPartModel>();

                    foreach (var tech in techs)
                    {
                        var mileages = (await Default.ListLoadAsync<MileageModel>(Default.GetMileagesSql(techs))).ToList();

                        var dateTimeInstall = mileages.Min(item => item.DateTime);
                        var dateTimeRemove = dateTimeInstall.AddDays(2);

                        for (var i = 0; i < TechPartsCount; i++)
                        {
                            var part = GetRandomItem(parts);

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

                await FillTableTechParts();
            }
        }
#endif
    }
}
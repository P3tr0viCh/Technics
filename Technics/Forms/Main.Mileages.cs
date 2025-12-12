using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        private async Task LoadMileagesAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var mileagesList = await ListLoadAsync<MileageModel>();

                mileagesList.ForEach(m =>
                {
                    m.TechText = Lists.Default.Techs.Find(t => t.Id == m.TechId)?.Text;
                });

                bindingSourceMileages.DataSource = mileagesList;

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private TechModel GetRandomTech()
        {
            return Lists.Default.Techs?[new Random().Next(Lists.Default.Techs.Count)];
        }

        private async Task MileageAddNewAsync()
        {
            var tech = GetRandomTech();

            var mileage = new MileageModel()
            {
                TechId = tech.Id,
                TechText = tech.Text,
                DateTime = DateTime.Now,
                Mileage = new Random().NextDouble() * 100.0,
            };

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await Database.Default.ListItemSaveAsync(mileage);

                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, nameof(MileageModel)));

                bindingSourceMileages.Insert(0, mileage);

                bindingSourceMileages.Position = 0;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            dgvMileages.Focus();
        }
    }
}
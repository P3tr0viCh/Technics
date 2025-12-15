using P3tr0viCh.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        private async Task MileagesLoadAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var mileagesList = await ListLoadAsync<MileageModel>();

                foreach (var mileage in mileagesList)
                {
                    mileage.TechText = Lists.Default.Techs.Find(t => t.Id == mileage.TechId)?.Text;

                    mileage.MileageCommon = await Database.Default.GetMileageCommonAsync(mileage);
                }

                mileagesList = mileagesList.OrderByDescending(m => m.DateTime).ToList();

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

        private async Task MileagesUpdateMileageCommonAsync(MileageModel mileage)
        {
            var changedMileageCommonList = bindingSourceMileages
                .Cast<MileageModel>()
                .Where(m => m.TechId == mileage.TechId && m.DateTime >= mileage.DateTime);

            DebugWrite.Line(changedMileageCommonList.Count());

            foreach (var changedMileage in changedMileageCommonList)
            {
                changedMileage.MileageCommon = await Database.Default.GetMileageCommonAsync(changedMileage);
            }
        }

        private async Task MileagesAddNewItemAsync(MileageModel mileage)
        {
            var tech = GetSelectedTech() ?? GetRandomTech();

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                if (mileage.Mileage == default)
                {
                    var mileageCommon = await Database.Default.GetMileageCommonAsync(mileage);

                    var newMileageCommon = mileageCommon + 1 + new Random().Next(10);

                    mileage.Mileage = newMileageCommon - mileageCommon;
                }

                await Database.Default.ListItemSaveAsync(mileage);

                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, nameof(MileageModel)));

                bindingSourceMileages.Insert(0, mileage);

                bindingSourceMileages.Position = 0;

                await MileagesUpdateMileageCommonAsync(mileage);
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

        private async Task MileagesAddNewAsync()
        {
            var tech = GetSelectedTech() ?? GetRandomTech();

            var mileage = new MileageModel()
            {
                TechId = tech.Id,
                TechText = tech.Text,
                DateTime = DateTime.Now,
                Mileage = 1 + new Random().Next(10),
            };

            await MileagesAddNewItemAsync(mileage);
        }

        private async Task MileagesAddNewCommonAsync()
        {
            var tech = GetSelectedTech() ?? GetRandomTech();

            var mileage = new MileageModel()
            {
                TechId = tech.Id,
                TechText = tech.Text,
                DateTime = DateTime.Now,
                Mileage = default,
            };

            await MileagesAddNewItemAsync(mileage);
        }

        private async Task MileagesDeleteSelectedAsync()
        {
            var mileage = ((BindingSource)dgvMileages.DataSource).Current as MileageModel;

            Utils.SetSelectedRows(dgvMileages, mileage);

            if (!Msg.Question(Resources.QuestionDeleteItem, mileage.DateTime)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await ListItemDeleteAsync(mileage);

                bindingSourceMileages.Remove(mileage);

                await MileagesUpdateMileageCommonAsync(mileage);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemDeleteFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            dgvMileages.Focus();
        }
    }
}
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
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
        private async Task MileagesLoadAsync(List<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                techs.ForEach(tech => DebugWrite.Line(tech.Text));

                var mileagesList = await ListLoadAsync<MileageModel>(GetMileagesQuery(techs));

                foreach (var mileage in mileagesList)
                {
                    mileage.TechText = Lists.Default.Techs.Find(t => t.Id == mileage.TechId)?.Text;

                    mileage.MileageCommon = await Database.Default.GetMileageCommonAsync(mileage);
                }

                bindingSourceMileages.DataSource = mileagesList;

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private IEnumerable<MileageModel> MileageList => bindingSourceMileages.Cast<MileageModel>();

        public MileageModel MileageSelected
        {
            get => ((BindingSource)dgvMileages.DataSource).Current as MileageModel;
        }

        private void MileagesUpdateTechText(TechModel tech)
        {
            var changedList = MileageList.Where(m => m.TechId == tech.Id);

            DebugWrite.Line(changedList.Count());

            if (!changedList.Any()) return;

            foreach (var changed in changedList)
            {
                changed.TechText = tech.Text;
            }

            dgvMileages.Refresh();
        }

        private async Task MileagesUpdateMileageCommonAsync(MileageModel mileage)
        {
            var changedList = MileageList.Where(m => m.TechId == mileage.TechId && m.DateTime >= mileage.DateTime);

            DebugWrite.Line(changedList.Count());

            if (!changedList.Any()) return;

            foreach (var changed in changedList)
            {
                changed.MileageCommon = await Database.Default.GetMileageCommonAsync(changed);
            }

            dgvMileages.Refresh();
        }

        private async Task MileagesChangeAsync(MileageModel mileage)
        {
            if (!FrmMileage.ShowDlg(this, mileage)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var isNew = mileage.IsNew;

                await Database.Default.ListItemSaveAsync(mileage);

                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, nameof(MileageModel)));

                if (isNew)
                {
                    bindingSourceMileages.Insert(0, mileage);

                    bindingSourceMileages.Position = 0;
                }
                else
                {
                    dgvMileages.Refresh();
                }

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
            var mileage = new MileageModel()
            {
                DateTime = DateTime.Now,
            };

            var tech = SelectedTech;

            if (tech != null)
            {
                mileage.TechId = tech.Id;
                mileage.TechText = tech.Text;
            }

            await MileagesChangeAsync(mileage);
        }

        private async Task MileagesChangeSelectedAsync()
        {
            var mileage = MileageSelected;

            if (mileage == null) return;

            Utils.SetSelectedRows(dgvMileages, mileage);

            await MileagesChangeAsync(mileage);
        }

        private async Task MileagesDeleteSelectedAsync()
        {
            var mileage = MileageSelected;

            if (mileage == null) return;

            Utils.SetSelectedRows(dgvMileages, mileage);

            if (!Msg.Question(Resources.QuestionDeleteMileage, mileage.DateTime)) return;

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
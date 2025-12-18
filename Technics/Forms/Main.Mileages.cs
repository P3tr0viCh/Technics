using P3tr0viCh.Database;
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

                var mileageList = await ListLoadAsync<MileageModel>(GetMileagesSql(techs));

                foreach (var mileage in mileageList)
                {
                    //mileage.TechText = Lists.Default.Techs.Find(t => t.Id == mileage.TechId)?.Text;

                    mileage.MileageCommon = await Database.Default.GetMileageCommonAsync(mileage);
                }

                bindingSourceMileages.DataSource = mileageList;

                MileagesListChanged();

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
            get => bindingSourceMileages.Current as MileageModel;
        }

        private void MileagesListChanged()
        {
            tsbtnMileagesDelete.Enabled = tsbtnMileagesChange.Enabled = bindingSourceMileages.Count > 0;
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

                    MileagesListChanged();
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

            dgvMileages.SetSelectedRows(mileage);

            await MileagesChangeAsync(mileage);
        }

        private async Task MileagesDeleteSelectedAsync()
        {
            var mileage = MileageSelected;

            if (mileage == null) return;

            dgvMileages.SetSelectedRows(mileage);

            if (!Msg.Question(Resources.QuestionMileageDelete,
                    mileage.DateTime.ToString(AppSettings.Default.FormatDateTime))) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await ListItemDeleteAsync(mileage);

                bindingSourceMileages.Remove(mileage);

                MileagesListChanged();

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
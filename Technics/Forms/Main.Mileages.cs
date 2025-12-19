using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        private async Task MileagesLoadAsync(IEnumerable<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                var list = await ListLoadAsync<MileageModel>(Database.Default.GetMileagesSql(techs));

                bindingSourceMileages.DataSource = list;

                await MileagesUpdateMileageCommonAsync(list);

                MileagesListChanged();

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private IEnumerable<MileageModel> MileageList => bindingSourceMileages.Cast<MileageModel>();

        public MileageModel MileageSelected => bindingSourceMileages.Current as MileageModel;

        private void MileagesListChanged()
        {
            tsbtnMileageDelete.Enabled = tsbtnMileageChange.Enabled = bindingSourceMileages.Count > 0;
        }

        private async Task MileagesUpdateMileageCommonAsync(IEnumerable<MileageModel> list)
        {
            DebugWrite.Line($"count={list.Count()}");

            if (!list.Any()) return;

            foreach (var item in list)
            {
                item.MileageCommon = await Database.Default.GetMileageCommonAsync(item);
            }

            dgvMileages.Refresh();
        }

        private async Task MileagesUpdateMileageCommonAsync(MileageModel mileage)
        {
            var list = MileageList.Where(m => m.TechId == mileage.TechId && m.DateTime >= mileage.DateTime);

            await MileagesUpdateMileageCommonAsync(list);
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

                await TechPartsUpdateMileageAsync(mileage);
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

                await TechPartsUpdateMileageAsync(mileage);
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
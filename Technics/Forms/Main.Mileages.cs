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
        internal readonly PresenterDataGridViewMileages presenterDataGridViewMileages;

        private async Task MileagesLoadAsync(IEnumerable<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                var list = techs.Count() > 0
                    ? await Database.Default.ListLoadAsync<MileageModel>(Database.Default.GetMileagesSql(techs))
                    : Enumerable.Empty<MileageModel>();

                bindingSourceMileages.DataSource = list;

                presenterDataGridViewMileages.Sort();

                bindingSourceMileages.Position = 0;

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
            get => presenterDataGridViewMileages.Selected; 
            set => presenterDataGridViewMileages.Selected = value;
        }
        
        public IEnumerable<MileageModel> MileageSelectedList
        {
            get => presenterDataGridViewMileages.SelectedList;
            set => presenterDataGridViewMileages.SelectedList = value;
        }

        private void MileagesListChanged()
        {
            tsbtnMileageDelete.Enabled = tsbtnMileageChange.Enabled = bindingSourceMileages.Count > 0;

            statusStripPresenter.MileageCount = bindingSourceMileages.Count;
        }

        private void MileagesUpdateChanged(UpdateModel changes)
        {
            DebugWrite.Line(changes.Mileages.Count);

            foreach (var mileage in MileageList)
            {
                var changed = changes.Mileages.Where(item => item.Id == mileage.Id).FirstOrDefault();

                if (changed == null) continue;

                mileage.MileageCommon = changed.MileageCommon;
            }
        }

        private async Task MileagesChangeAsync(MileageModel mileage)
        {
            if (!FrmMileage.ShowDlg(this, mileage)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var isNew = mileage.IsNew;

                var changes = await Database.Default.MileageSaveAsync(mileage);

                if (isNew)
                {
                    var pos = bindingSourceMileages.Add(mileage);

                    bindingSourceMileages.Position = pos;
                }
                else
                {
                    dgvMileages.Refresh();
                }

                presenterDataGridViewMileages.Sort();

                dgvMileages.SetSelectedRows(mileage);

                MileagesUpdateChanged(changes);

                TechPartsUpdateChanged(changes);

                MileagesListChanged();
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
#if DEBUG
                Mileage = 10,
#endif
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

            if (!Utils.Msg.Question(Resources.QuestionMileageDelete,
                    mileage.DateTime.ToString(AppSettings.Default.FormatDateTime))) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var changedList = await Database.Default.MileageDeleteAsync(mileage);

                bindingSourceMileages.Remove(mileage);

                dgvMileages.SetSelectedRows(dgvMileages.GetSelected<MileageModel>());

                MileagesUpdateChanged(changedList);

                TechPartsUpdateChanged(changedList);

                MileagesListChanged();
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
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Technics.Presenters;
using Technics.Properties;
using static P3tr0viCh.Utils.Gpx;
using static Technics.Database.Models;
using static Technics.ProgramStatus;

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
                var list = await Database.Default.MileagesLoadAsync(techs);

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

            presenterStatusStrip.MileageCount = bindingSourceMileages.Count;
        }

        private void MileagesUpdateChanged(UpdateModel changes)
        {
            DebugWrite.Line(changes.Mileages.Count);

            foreach (var mileage in MileageList)
            {
                var changed = changes.Mileages.Where(item => item.Id == mileage.Id).FirstOrDefault();

                if (changed == null) continue;

                mileage.Mileage = changed.Mileage;
                mileage.MileageCommon = changed.MileageCommon;
            }
        }

        private async Task MileagesChangeAsync(MileageModel mileage)
        {
            if (!FrmMileage.ShowDlg(this, mileage)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var isNew = mileage.IsNew;

                var updated = await Database.Default.MileageSaveAsync(mileage);

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

                MileagesUpdateChanged(updated);

                TechPartsUpdateChanged(updated);

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
                ProgramStatus.Default.Stop(status);
            }

            dgvMileages.Focus();
        }

        private async Task MileagesChangeAsync(IEnumerable<MileageModel> mileages)
        {
            if (!FrmMileageList.ShowDlg(this, mileages)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var updated = await Database.Default.MileageSaveAsync(mileages);

                dgvMileages.Refresh();

                presenterDataGridViewMileages.Sort();

                dgvMileages.SetSelectedRows(mileages);

                MileagesUpdateChanged(updated);

                TechPartsUpdateChanged(updated);

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
                ProgramStatus.Default.Stop(status);
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
            var mileages = MileageSelectedList;

            if (!mileages.Any()) return;

            dgvMileages.SetSelectedRows(mileages);

            if (mileages.Count() == 1)
            {
                await MileagesChangeAsync(mileages.FirstOrDefault());
            }
            else
            {
                await MileagesChangeAsync(mileages);
            }
        }

        private async Task MileagesDeleteSelectedAsync()
        {
            var mileages = MileageSelectedList;

            if (!mileages.Any()) return;

            dgvMileages.SetSelectedRows(mileages);

            if (!Utils.Msg.Question(mileages)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var updated = await Database.Default.MileageDeleteAsync(mileages);

                foreach (var mileage in mileages)
                {
                    bindingSourceMileages.Remove(mileage);
                }

                dgvMileages.SetSelectedRows(dgvMileages.GetSelectedList<MileageModel>());

                MileagesUpdateChanged(updated);

                TechPartsUpdateChanged(updated);

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
                ProgramStatus.Default.Stop(status);
            }

            dgvMileages.Focus();
        }

        private static DataTableFile MileagesCreateDataTableFile()
        {
            var table = new DataTable("Mileages");

            table.Columns.Add("Tech", typeof(string));
            table.Columns.Add("DateTime", typeof(DateTime));
            table.Columns.Add("Mileage", typeof(double));
            table.Columns.Add("Description", typeof(string));

            return new DataTableFile(table);
        }

        private async Task<IEnumerable<MileageModel>> MileagesLoadFromFileCsvAsync(string fileName)
        {
            var dataTableFile = MileagesCreateDataTableFile();

            dataTableFile.FileName = fileName;

            await Task.Factory.StartNew(() =>
            {
                dataTableFile.ReadFromCsv();
            });

            var mileages = new List<MileageModel>();

            foreach (DataRow row in dataTableFile.Table.Rows)
            {
                var tech = Lists.Default.Techs.Find(t => t.Text == row.AsString("Tech"));

                var mileage = new MileageModel()
                {
                    TechId = tech?.Id,
                    DateTime = row.AsDateTime("DateTime"),
                    Mileage = row.AsDouble("Mileage"),
                    Description = row.AsStringNullable("Description"),
                };

                mileages.Add(mileage);
            }

            return mileages;
        }

        private async Task<MileageModel> MileagesLoadFromFileGpxAsync(string fileName)
        {
            var gpx = new Track();

            await gpx.OpenFromFileAsync(fileName);

            var mileage = new MileageModel
            {
                DateTime = gpx.DateTimeStart,
                Mileage = gpx.Distance / 1000.0,
                Description = gpx.Text
            };

            return mileage;
        }

        private async Task SelectMileagesAsync(IEnumerable<MileageModel> mileages)
        {
            if (!mileages.Any()) return;

            if (tvTechs.Nodes[0].IsSelected)
            {
                await UpdateDataAsync(DataLoad.Mileages | DataLoad.TechParts);
            }

            tvTechs.SelectedNode = tvTechs.Nodes[0];

            MileageSelectedList = mileages;
        }
    }
}
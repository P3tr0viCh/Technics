using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Presenters;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.ProgramStatus;

namespace Technics
{
    public partial class Main
    {
        internal readonly WrapperCancellationTokenSource ctsMaintenanceLoad = new WrapperCancellationTokenSource();

        internal readonly PresenterDataGridViewMaintenance presenterDataGridViewMaintenance;

        private async Task MaintenanceLoadAsync(IEnumerable<TechModel> techs)
        {
            DebugWrite.Line("start");

            ctsMaintenanceLoad.Start();

            try
            {
                var list = await Database.Default.MaintenanceLoadAsync(techs);

                if (ctsMaintenanceLoad.IsCancellationRequested) return;

                bindingSourceMaintenance.DataSource = list;

                presenterDataGridViewMaintenance.Sort();

                bindingSourceMaintenance.Position = 0;

                MaintenanceListChanged();

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            finally
            {
                ctsMaintenanceLoad.Finally();

                DebugWrite.Line("end");
            }
        }

        private IEnumerable<MaintenanceModel> MaintenanceList => bindingSourceMaintenance.Cast<MaintenanceModel>();

        public MaintenanceModel MaintenanceSelected
        {
            get => presenterDataGridViewMaintenance.Selected;
            set => presenterDataGridViewMaintenance.Selected = value;
        }

        public IEnumerable<MaintenanceModel> MaintenanceSelectedList
        {
            get => presenterDataGridViewMaintenance.SelectedList;
            set => presenterDataGridViewMaintenance.SelectedList = value;
        }

        private void MaintenanceListChanged()
        {
            tsbtnMaintenanceDelete.Enabled =
            tsbtnMaintenanceChange.Enabled =
            miMaintenanceDelete.Enabled =
            miMaintenanceChange.Enabled =
                bindingSourceMaintenance.Count > 0;

            presenterStatusStrip.Maintenance.Count = bindingSourceMaintenance.Count;
        }

        private async Task MaintenanceChangeAsync(MaintenanceModel maintenance)
        {
            if (!FrmMaintenance.ShowDlg(this, maintenance)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var isNew = maintenance.IsNew;

                var changes = await Database.Default.MaintenanceSaveAsync(maintenance);

                if (isNew)
                {
                    var pos = bindingSourceMaintenance.Add(maintenance);

                    bindingSourceMaintenance.Position = pos;
                }
                else
                {
                    dgvMaintenance.Refresh();
                }

                presenterDataGridViewMaintenance.Sort();

                dgvMaintenance.SetSelectedRows(maintenance);

                // TODO: MaintenanceUpdateChanged(changes);

                MaintenanceListChanged();
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

            dgvMaintenance.Focus();
        }

        private async Task MaintenanceAddNewAsync()
        {
            var maintenance = new MaintenanceModel()
            {
                DateTime = DateTime.Now,
            };

            var tech = SelectedTech;

            if (tech != null)
            {
                maintenance.TechId = tech.Id;
                maintenance.TechText = tech.Text;
            }

            await MaintenanceChangeAsync(maintenance);
        }

        private async Task MaintenanceChangeSelectedAsync()
        {
            var maintenance = MaintenanceSelected;

            if (maintenance == null) return;

            dgvMaintenance.SetSelectedRows(maintenance);

            await MaintenanceChangeAsync(maintenance);
        }

        private async Task MaintenanceDeleteSelectedAsync()
        {
            var maintenances = MaintenanceSelectedList;

            if (!maintenances.Any()) return;

            dgvMaintenance.SetSelectedRows(maintenances);

            if (!Utils.Msg.Question(maintenances)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var updated = await Database.Default.MaintenanceDeleteAsync(maintenances);

                foreach (var maintenance in maintenances)
                {
                    bindingSourceMaintenance.Remove(maintenance);
                }

                dgvMaintenance.SetSelectedRows(dgvMaintenance.GetSelected<MaintenanceModel>());

                // TODO: MaintenanceUpdateChanged(updated);

                MaintenanceListChanged();
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

            dgvMaintenance.Focus();
        }
    }
}
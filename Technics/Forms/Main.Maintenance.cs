using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Presenters;
using Technics.Properties;
using static Technics.Database.Models;

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
    }
}
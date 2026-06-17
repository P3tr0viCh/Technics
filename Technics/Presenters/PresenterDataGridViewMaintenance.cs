using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Presenters;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterDataGridViewMaintenance : PresenterDataGridView<MaintenanceModel>
    {
        public PresenterDataGridViewMaintenance(DataGridView dataGridView) : base(dataGridView)
        {
        }

        public override int Compare(MaintenanceModel x, MaintenanceModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(MaintenanceModel.TechText):
                    result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.MileageCommon, y.MileageCommon, ComparerSortOrder.Ascending);
                    break;
                case nameof(MaintenanceModel.MtText):
                    result = EmptyStringComparer.Default.Compare(x.MtText, y.MtText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.MileageCommon, y.MileageCommon, ComparerSortOrder.Ascending);
                    break;
                case nameof(MaintenanceModel.MileageCommon):
                    result = SortOrderComparer.Default.Compare(x.MileageCommon, y.MileageCommon, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    break;
                case nameof(MaintenanceModel.DateTime):
                    result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.MtText, y.MtText, ComparerSortOrder.Ascending);
                    break;
                case nameof(MaintenanceModel.MileageAfterMaintenance):
                    result = SortOrderComparer.Default.Compare(x.MileageAfterMaintenance, y.MileageAfterMaintenance, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.MtText, y.MtText, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}
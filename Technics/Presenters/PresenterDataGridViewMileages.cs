using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Presenters;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterDataGridViewMileages : PresenterDataGridView<MileageModel>
    {
        public PresenterDataGridViewMileages(DataGridView dataGridView) : base(dataGridView)
        {
        }

        public override int Compare(MileageModel x, MileageModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(MileageModel.TechText):
                    result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, ComparerSortOrder.Ascending);
                    break;
                case nameof(MileageModel.Mileage):
                    result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    break;
                case nameof(MileageModel.DateTime):
                    result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, ComparerSortOrder.Ascending);
                    break;
                case nameof(MileageModel.MileageCommon):
                    result = SortOrderComparer.Default.Compare(x.MileageCommon, y.MileageCommon, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    break;
                case nameof(MileageModel.Description):
                    result = EmptyStringComparer.Default.Compare(x.Description, y.Description, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTime, y.DateTime, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}
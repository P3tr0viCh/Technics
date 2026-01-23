using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using System.Collections;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterDataGridViewTechParts : PresenterDataGridView<TechPartModel>
    {
        public PresenterDataGridViewTechParts(DataGridView dataGridView) : base(dataGridView)
        {
        }

        public override int Compare(TechPartModel x, TechPartModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(TechPartModel.TechText):
                    result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeInstall, y.DateTimeInstall, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, ComparerSortOrder.Ascending);
                    break;
                case nameof(TechPartModel.PartText):
                    result = EmptyStringComparer.Default.Compare(x.PartText, y.PartText, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeInstall, y.DateTimeInstall, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, ComparerSortOrder.Ascending);
                    break;
                case nameof(TechPartModel.Mileage):
                    result = SortOrderComparer.Default.Compare(x.Mileage, y.Mileage, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.DateTimeInstall, y.DateTimeInstall, ComparerSortOrder.Descending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    break;
                case nameof(TechPartModel.DateTimeInstall):
                    result = SortOrderComparer.Default.Compare(x.DateTimeInstall, y.DateTimeInstall, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.PartText, y.PartText, ComparerSortOrder.Ascending);
                    break;
                case nameof(TechPartModel.DateTimeRemove):
                    result = SortOrderComparer.Default.Compare(x.DateTimeRemove, y.DateTimeRemove, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.PartText, y.PartText, ComparerSortOrder.Ascending);
                    break;
                case nameof(TechPartModel.MileageCommon):
                    result = SortOrderComparer.Default.Compare(x.MileageCommon, y.MileageCommon, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.TechText, y.TechText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.PartText, y.PartText, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}
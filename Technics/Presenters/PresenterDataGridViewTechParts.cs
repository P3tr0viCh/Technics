using P3tr0viCh.Utils;
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

        public override int Compare(TechPartModel x, TechPartModel y, string dataPropertyName)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(TechPartModel.TechText):
                    result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(y.DateTimeInstall, x.DateTimeInstall);
                    if (result == 0) result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    break;
                case nameof(TechPartModel.PartText):
                    result = Comparer.Default.Compare(x.PartText, y.PartText);
                    if (result == 0) result = Comparer.Default.Compare(y.DateTimeInstall, x.DateTimeInstall);
                    if (result == 0) result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    break;
                case nameof(TechPartModel.Mileage):
                    result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    if (result == 0) result = Comparer.Default.Compare(y.DateTimeInstall, x.DateTimeInstall);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    break;
                case nameof(TechPartModel.DateTimeInstall):
                    result = Comparer.Default.Compare(x.DateTimeInstall, y.DateTimeInstall);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.PartText, y.PartText);
                    break;
                case nameof(TechPartModel.DateTimeRemove):
                    result = Comparer.Default.Compare(x.DateTimeRemove, y.DateTimeRemove);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.PartText, y.PartText);
                    break;
                case nameof(TechPartModel.MileageCommon):
                    result = Comparer.Default.Compare(x.MileageCommon, y.MileageCommon);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.PartText, y.PartText);
                    break;
            }

            return result;
        }
    }
}
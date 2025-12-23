using P3tr0viCh.Utils;
using System.Collections;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics
{
    internal class PresenterDataGridViewMileages : PresenterDataGridView<MileageModel>
    {
        public PresenterDataGridViewMileages(DataGridView dataGridView) : base(dataGridView)
        {
        }

        protected override int Compare(MileageModel x, MileageModel y, string dataPropertyName)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(MileageModel.TechText):
                    result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.DateTime, y.DateTime);
                    if (result == 0) result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    break;
                case nameof(MileageModel.Mileage):
                    result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    if (result == 0) result = Comparer.Default.Compare(x.DateTime, y.DateTime);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    break;
                case nameof(MileageModel.DateTime):
                    result = Comparer.Default.Compare(x.DateTime, y.DateTime);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.Mileage, y.Mileage);
                    break;
                case nameof(MileageModel.MileageCommon):
                    result = Comparer.Default.Compare(x.MileageCommon, y.MileageCommon);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    if (result == 0) result = Comparer.Default.Compare(x.DateTime, y.DateTime);
                    break;
                case nameof(MileageModel.Description):
                    result = Comparer.Default.Compare(x.Description, y.Description);
                    if (result == 0) result = Comparer.Default.Compare(x.DateTime, y.DateTime);
                    if (result == 0) result = Comparer.Default.Compare(x.TechText, y.TechText);
                    break;
            }

            return result;
        }
    }
}
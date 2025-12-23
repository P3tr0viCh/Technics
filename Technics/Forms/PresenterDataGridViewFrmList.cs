using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections;
using System.Windows.Forms;

namespace Technics
{
    internal class PresenterDataGridViewFrmList : PresenterDataGridView<IBaseText>
    {
        public PresenterDataGridViewFrmList(DataGridView dataGridView) : base(dataGridView)
        {
        }

        protected override int Compare(IBaseText x, IBaseText y, string dataPropertyName)
        {
            return Comparer.Default.Compare(x.Text, y.Text);
        }
    }
}
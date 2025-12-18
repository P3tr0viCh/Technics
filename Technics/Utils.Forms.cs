using P3tr0viCh.Utils;
using System.Windows.Forms;
using Technics.Properties;

namespace Technics
{
    public static partial class Utils
    {
        public static void SelectCellOnCellMouseDown(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                }
            }
        }

        public static bool TextInputBoxShow(ref string text, string caption)
        {
            return TextInputBox.Show(ref text, new TextInputBox.Settings()
            {
                Caption = caption,
                Label = Resources.TextLabelText
            });
        }

        public static void SetShowTextAndToolTips(this ToolStrip toolStrip, bool value)
        {
            toolStrip.SetShowText(value);
            toolStrip.ShowItemToolTips = !value;
        }
    }
}
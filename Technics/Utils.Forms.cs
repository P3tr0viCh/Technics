using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Technics.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Technics
{
    public static partial class Utils
    {
        public static void SetSelectedRows(DataGridView dataGridView, List<BaseId> values)
        {
            dataGridView.ClearSelection();

            foreach (var value in values)
            {
                foreach (var row in from DataGridViewRow row in dataGridView.Rows
                                    where (row.DataBoundItem as BaseId).Id == value.Id
                                    select row)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        public static void SetSelectedRows(DataGridView dataGridView, BaseId value)
        {
            SetSelectedRows(dataGridView, new List<BaseId>() { value });
        }

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
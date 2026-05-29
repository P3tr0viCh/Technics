using P3tr0viCh.Database;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using System;
using System.Windows.Forms;
using Technics.Properties;

namespace Technics
{
    public static partial class Utils
    {
        public static void ShowMenuOnCellMouseClick(ContextMenuStrip contextMenu, DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                contextMenu.Show(dataGridView, dataGridView.PointToClient(Cursor.Position));
            }
        }
        
        public static void ShowMenuOnNodeMouseClick(ContextMenuStrip contextMenu, TreeView treeView, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node != null)
            {
                treeView.SelectedNode = e.Node;

                contextMenu.Show(treeView, treeView.PointToClient(Cursor.Position));
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

        public static void AssertTextEmpty(TextBox textBox)
        {
            if (textBox.IsEmpty())
            {
                textBox.Focus();

                throw new Exception(Resources.ErrorValueNeedText);
            }
        }

        public static void AssertComboBox<T>(ComboBox comboBox, string error) where T : BaseId
        {
            var item = comboBox.GetSelectedItem<T>();

            if (item.IsNew)
            {
                comboBox.Focus();

                throw new Exception(error);
            }
        }
    }
}
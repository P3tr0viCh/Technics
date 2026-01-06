using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
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

        public static void AssertComboBox<T>(ComboBox comboBox, string error) where T : BaseId
        {
            var item = comboBox.GetSelectedItem<T>();

            if (item.IsNew)
            {
                comboBox.Focus();

                throw new Exception(error);
            }
        }

        public static ContextMenuStrip CreateMenuDateTimePicker()
        {
            var menuDateTimePicker = new ContextMenuStrip();

            var menuItemCopy = new ToolStripMenuItem()
            {
                Text = Resources.TextMenuItemCopy
            };

            menuItemCopy.Click += MenuItemCopy_Click;

            menuDateTimePicker.Items.Add(menuItemCopy);

            var menuItemPaste = new ToolStripMenuItem()
            {
                Text = Resources.TextMenuItemPaste
            };

            menuItemPaste.Click += MenuItemPaste_Click;

            menuDateTimePicker.Items.Add(menuItemPaste);

            return menuDateTimePicker;
        }

        private static DateTimePicker GetDateTimePickerFromMenuItem(object sender)
        {
            var menuItem = sender as ToolStripMenuItem;

            var contextMenuStrip = menuItem.GetCurrentParent() as ContextMenuStrip;

            if (contextMenuStrip.SourceControl is DateTimePicker dateTimePicker)
            {
                return dateTimePicker;
            }

            return null;
        }

        public static void MenuItemCopy_Click(object sender, EventArgs e)
        {
            var dateTimePicker = GetDateTimePickerFromMenuItem(sender);

            dateTimePicker?.CopyToClipboard();
        }

        private static void MenuItemPaste_Click(object sender, EventArgs e)
        {
            var dateTimePicker = GetDateTimePickerFromMenuItem(sender);

            dateTimePicker?.PasteFromClipboard();
        }
    }
}
using System.Windows.Forms;

namespace Technics
{
    internal interface IFrmList
    {
        IMainForm MainForm { get; }

        DataGridView DataGridView { get; }

        BindingSource BindingSource { get; }

        ToolStrip ToolStrip { get; }
    }
}
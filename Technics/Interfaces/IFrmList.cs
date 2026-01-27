using System.Windows.Forms;

namespace Technics.Interfaces
{
    internal interface IFrmList
    {
        IMainForm MainForm { get; }

        DataGridView DataGridView { get; }

        ToolStrip ToolStrip { get; }
    }
}
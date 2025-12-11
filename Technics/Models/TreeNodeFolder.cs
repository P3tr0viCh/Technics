using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeFolder : TreeNode
    {
        public Folder Folder { get; set; } = new Folder();

        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Folder.Text = value;
            }
        }
    }
}
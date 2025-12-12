using P3tr0viCh.Database;
using System.Windows.Forms;

namespace Technics.Models
{
    internal abstract class TreeNodeBase : TreeNode
    {
        private BaseText model;
        public BaseText Model
        {
            get => model;
            set
            {
                model = value;

                Text = model?.Text;
            }
        }

        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;

                model.Text = value;
            }
        }
    }
}
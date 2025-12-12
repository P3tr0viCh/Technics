using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeTech : TreeNodeBase
    {
        public Tech Tech { get => (Tech)Model; set => Model = value; }

        public TreeNodeTech(Tech folder)
        {
            Tech = folder;

            ImageIndex = 1;
            SelectedImageIndex = 1;
        }

        public TreeNodeTech() : this(new Tech())
        {
        }
    }
}
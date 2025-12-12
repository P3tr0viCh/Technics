using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeTech : TreeNodeBase
    {
        public TechModel Tech { get => (TechModel)Model; set => Model = value; }

        public TreeNodeTech(TechModel folder)
        {
            Tech = folder;

            ImageIndex = 1;
            SelectedImageIndex = 1;
        }

        public TreeNodeTech() : this(new TechModel())
        {
        }
    }
}
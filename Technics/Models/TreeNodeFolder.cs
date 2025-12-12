using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeFolder : TreeNodeBase
    {
        public Folder Folder { get => (Folder)Model; set => Model = value; }

        public TreeNodeFolder(Folder folder)
        {
            Folder = folder;

            ImageIndex = 0;
            SelectedImageIndex = 0;
        }

        public TreeNodeFolder() : this(new Folder())
        {
        }
    }
}
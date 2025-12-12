using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeFolder : TreeNodeBase
    {
        public FolderModel Folder { get => (FolderModel)Model; set => Model = value; }

        public TreeNodeFolder(FolderModel folder)
        {
            Folder = folder;

            ImageIndex = 0;
            SelectedImageIndex = 0;
        }

        public TreeNodeFolder() : this(new FolderModel())
        {
        }
    }
}
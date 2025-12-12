using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TreeNodeFolder : TreeNodeBase
    {
        public Folder Folder { get => (Folder)Model; set => Model = value; }

        public TreeNodeFolder()
        {
            Folder = new Folder();
        }

        public TreeNodeFolder(Folder folder)
        {
            Folder = folder;
        }
    }
}
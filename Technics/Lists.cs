using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Technics.Models;
using static Technics.Database.Models;

namespace Technics
{
    internal class TechList : BaseIdList<TechModel>
    {
        public TechList()
        {
        }

        public TechList(IEnumerable<TechModel> collection) : base(collection)
        {
        }
    }

    internal class FolderList : BaseIdList<FolderModel>
    {
        public FolderList()
        {
        }

        public FolderList(IEnumerable<FolderModel> collection) : base(collection)
        {
            ListChanged();
        }

        private void GetParentFolderList(long? id, ref List<FolderModel> folders)
        {
            var folder = Find(id);

            if (folder == null) return;

            GetParentFolderList(folder?.ParentId, ref folders);

            folders.Add(folder);
        }

        public IEnumerable<FolderModel> GetFolderList(long? id)
        {
            var folders = new List<FolderModel>();

            GetParentFolderList(id, ref folders);

            return Enumerable.Empty<FolderModel>().Concat(folders);
        }

        private string GetFolderPath(IEnumerable<FolderModel> folders)
        {
            return string.Join(Path.DirectorySeparatorChar.ToString(), folders.Select(f => f.Text));
        }

        private void ListChanged()
        {
            ForEach(item => item.Path = GetFolderPath(GetFolderList(item.Id)));
        }

        public new void Add(FolderModel item)
        {
            item.Path = GetFolderPath(GetFolderList(item.Id));

            base.Add(item);
        }

        public new bool Remove(FolderModel item)
        {
            if (!base.Remove(item)) return false;

            ListChanged();

            return true;
        }

        public void Replace(FolderModel item)
        {
            if (!base.Remove(item)) return;

            Add(item);
        }
    }

    internal class Lists : DefaultInstance<Lists>
    {
        public TechList Techs { get; set; } = new TechList();

        public FolderList Folders { get; set; } = new FolderList();
    }
}
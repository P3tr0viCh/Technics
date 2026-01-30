using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;

namespace Technics.Models
{
    internal class BaseIdList<T> : List<T> where T : IBaseId
    {
        public BaseIdList() { }

        public BaseIdList(IEnumerable<T> collection)
        {
            AddRange(collection);
        }

        public T Find(long? id)
        {
            if (id == null || id == Sql.NewId) return default;

            return Find(item => item.Id == id);
        }
    }
}
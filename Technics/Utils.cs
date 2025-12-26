using P3tr0viCh.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Technics
{
    public static partial class Utils
    {
        public const int ListEditId = -1;

        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> list) where T : BaseId
        {
            return new BindingList<T>(list.ToList());
        }
    }
}
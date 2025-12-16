using P3tr0viCh.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Technics
{
    public static partial class Utils
    {
        public static BindingList<T> ToBindingList<T>(this List<T> list) where T : BaseId
        {
            return new BindingList<T>(list.ToList());
        }
    }
}
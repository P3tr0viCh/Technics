using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Technics
{
    public static partial class Utils
    {
        public const int ListEditId = -1;

        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> list) where T : BaseId
        {
            return new BindingList<T>(list.ToList());
        }

        public static IEnumerable<long> DistinctNotNullLong<T>(this IEnumerable<T> values)
        {
            return values.Distinct().Where(i => i != null).Cast<long>();
        }

        public static void ListAddNotNull(List<long> list, long? item)
        {
            if (item != null)
            {
                list.Add((long)item);
            }
        }

        public static async Task<string[]> EnumerateFilesAsync(string path, string extensions)
        {
            var files = await Files.DirectoryEnumerateFilesAsync(path, SearchOption.AllDirectories);

            var exts = extensions.ToLower().Split(';');

            files = files.Where(file =>
                exts.Any(ext => ext == Path.GetExtension(file).ToLower()));

            return files.ToArray();
        }
    }
}
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using static Technics.Database.Interfaces;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Database
    {
        public static class Filter
        {
            public static string ByIdToString(string fieldName, IEnumerable<long> ids)
            {
                var count = ids.Count();

                if (count == 0) return string.Empty;

                string result;

                if (count == 1)
                {
                    result = $"{fieldName} = {ids.First()}";
                }
                else
                {
                    result = string.Join(", ", ids);

                    result = $"{fieldName} IN ({result})";
                }

                return result;
            }

            public static string ByIdToString(string fieldName, IEnumerable<IBaseId> list)
            {
                var count = list.Count();

                if (count == 0) return string.Empty;

                var ids = list.Select(x => x.Id);

                return ByIdToString(fieldName, ids);
            }
 
            private static string TechsToString(IEnumerable<TechModel> techs)
            {
                if (techs == null) return string.Empty;

                if (techs.Count() == Lists.Default.Techs.Count) return string.Empty;

                var fieldName = Sql.FieldName(nameof(ITechId.TechId));

                var result = ByIdToString(fieldName, techs);

                return result;
            }

            private static string PartsToString(IEnumerable<PartModel> parts)
            {
                if (parts == null) return string.Empty;

                var fieldName = Sql.FieldName(nameof(IPartId.PartId));

                var result = ByIdToString(fieldName, parts);

                return result;
            }

            public abstract class BaseFilter
            {
                public abstract override string ToString();
            }

            public class Mileages : BaseFilter
            {
                public IEnumerable<TechModel> Techs { get; set; } = null;

                public override string ToString()
                {
                    var result = TechsToString(Techs);

                    return result;
                }
            }

            public class TechParts : BaseFilter
            {
                public IEnumerable<TechModel> Techs { get; set; } = null;

                public IEnumerable<PartModel> Parts { get; set; } = null;

                public override string ToString()
                {
                    var result = TechsToString(Techs);

                    result = result.JoinExcludeEmpty(" AND ", PartsToString(Parts));

                    return result;
                }
            }
        }
    }
}
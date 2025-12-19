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
            private static string ByIdToString(string fieldName, List<BaseId> list)
            {
                var result = string.Empty;

                if (list.Count == 1)
                {
                    result = $"{fieldName} = {list[0].Id}";
                }
                else
                {
                    list.ForEach(item => result = result.JoinExcludeEmpty(", ", item.Id.ToString()));

                    result = $"{fieldName} IN ({result})";
                }

                return result;
            }

            private static string TechsToString(IEnumerable<TechModel> techs)
            {
                if (techs == null) return string.Empty;

                var list = techs.Cast<BaseId>().ToList();

                if (list.Count == 0) return string.Empty;

                if (list.Count == Lists.Default.Techs.Count) return string.Empty;

                var fieldName = Sql.FieldName(nameof(ITechId.TechId));

                var result = ByIdToString(fieldName, list);

                return result;
            }

            private static string PartsToString(IEnumerable<PartModel> parts)
            {
                if (parts == null) return string.Empty;

                var list = parts.Cast<BaseId>().ToList();

                if (list.Count == 0) return string.Empty;

                var fieldName = Sql.FieldName(nameof(IPartId.PartId));

                var result = ByIdToString(fieldName, list);

                return result;
            }

            public static string GetWhereSql(BaseFilter filter)
            {
                var where = filter.ToString();

                if (where.IsEmpty()) return string.Empty;

                return $"WHERE {where}";
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
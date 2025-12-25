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
            public static string ByIdToString(string fieldName, IEnumerable<long> list)
            {
                var count = list.Count();

                if (count == 0) return string.Empty;

                string result;

                if (count == 1)
                {
                    result = $"{fieldName} = {list.First()}";
                }
                else
                {
                    result = string.Join(", ", list);

                    result = $"{fieldName} IN ({result})";
                }

                return result;
            }

            private static string TechsToString(IEnumerable<TechModel> techs)
            {
                if (techs == null) return string.Empty;

                var list = techs.Select(item => item.Id);

                if (!list.Any()) return string.Empty;

                if (list.Count() == Lists.Default.Techs.Count) return string.Empty;

                var fieldName = Sql.FieldName(nameof(ITechId.TechId));

                var result = ByIdToString(fieldName, list);

                return result;
            }

            private static string PartsToString(IEnumerable<PartModel> parts)
            {
                if (parts == null) return string.Empty;

                var list = parts.Select(item => item.Id);

                if (!list.Any()) return string.Empty;

                var fieldName = Sql.FieldName(nameof(IPartId.PartId));

                var result = ByIdToString(fieldName, list);

                return result;
            }

            public abstract class BaseFilter
            {
                protected string GetWhereSql(string conditional)
                {
                    if (conditional.IsEmpty()) return string.Empty;

                    return $"WHERE {conditional}";
                }

                public abstract override string ToString();
            }

            public class Mileages : BaseFilter
            {
                public IEnumerable<TechModel> Techs { get; set; } = null;

                public override string ToString()
                {
                    var result = TechsToString(Techs);

                    result = GetWhereSql(result);

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

                    result = GetWhereSql(result);

                    return result;
                }
            }
        }
    }
}
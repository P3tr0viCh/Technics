using P3tr0viCh.Utils;
using System.Collections.Generic;
using static Technics.Database.Models;

namespace Technics
{
    internal class Lists : DefaultInstance<Lists>
    {
        public List<TechModel> Techs { get; set; } = new List<TechModel>();

        public TechModel FindTechByText(string text)
        {
            return Techs.Find(tech => tech.Text == text);
        }
    }
}
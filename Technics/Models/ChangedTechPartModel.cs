using System.Collections.Generic;
using static Technics.Database.Models;

namespace Technics
{
    public class ChangedTechPartModel
    {
        public List<TechPartModel> ChangedTechParts { get; set; } = new List<TechPartModel>();
        public List<TechPartModel> UpdatedTechParts { get; set; } = new List<TechPartModel>();
    }
}
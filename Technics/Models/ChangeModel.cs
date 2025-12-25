using System.Collections.Generic;

namespace Technics
{
    public class ChangeModel
    {
        public List<ChangeMileageModel> Mileages { get; set; } = new List<ChangeMileageModel>();
        public List<ChangeTechPartModel> TechParts { get; set; } = new List<ChangeTechPartModel>();
    }
}
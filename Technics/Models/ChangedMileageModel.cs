using System.Collections.Generic;
using static Technics.Database.Models;

namespace Technics
{
    public class ChangedMileageModel
    {
        public List<MileageModel> ChangedMileages { get; set; } = new List<MileageModel>();
        public List<MileageModel> UpdatedMileages { get; set; } = new List<MileageModel>();
    }
}
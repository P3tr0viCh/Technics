using System.Collections.Generic;

namespace Technics
{
    public class UpdateModel
    {
        public List<UpdateMileageModel> Mileages { get; set; } = new List<UpdateMileageModel>();
        public List<UpdateTechPartModel> TechParts { get; set; } = new List<UpdateTechPartModel>();
        public List<UpdateMaintenanceModel> Maintenance { get; set; } = new List<UpdateMaintenanceModel>();
    }
}
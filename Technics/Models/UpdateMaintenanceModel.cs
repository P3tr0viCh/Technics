using P3tr0viCh.Database;

namespace Technics
{
    public class UpdateMaintenanceModel : BaseId
    {
        public double? MileageCommon { get; set; } = null;
        public double? MileageAfterMaintenance { get; set; } = null;
    }
}
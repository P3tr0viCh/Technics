using P3tr0viCh.Database;

namespace Technics
{
    public class UpdateTechPartModel : BaseId
    {
        public double? Mileage { get; set; } = null;
        public double? MileageCommon { get; set; } = null;
    }
}
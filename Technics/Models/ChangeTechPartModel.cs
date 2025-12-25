using P3tr0viCh.Database;

namespace Technics
{
    public class ChangeTechPartModel : BaseId
    {
        public double? Mileage { get; set; } = null;
        public double? MileageCommon { get; set; } = null;
    }
}
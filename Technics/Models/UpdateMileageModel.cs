using P3tr0viCh.Database;

namespace Technics
{
    public class UpdateMileageModel : BaseId
    {
        public double Mileage { get; set; } = default;
        public double MileageCommon { get; set; } = default;
    }
}
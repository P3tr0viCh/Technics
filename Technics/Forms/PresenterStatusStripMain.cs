using P3tr0viCh.Utils;
using Technics.Properties;
using static Technics.Enums;

namespace Technics
{
    internal class PresenterStatusStripMain : PresenterStatusStrip<PresenterStatusStripMain.StatusLabel>
    {
        public enum StatusLabel
        {
            Status,
            MileageCount,
            TechPartCount,
        }

        public PresenterStatusStripMain(IPresenterStatusStrip view) : base(view)
        {
            Status = Status.Idle;
            MileageCount = 0;
            TechPartCount = 0;
        }

        public Status Status
        {
            set => View.GetLabel(StatusLabel.Status).Text = value.Description();
        }

        public int MileageCount
        {
            set => View.GetLabel(StatusLabel.MileageCount).Text = string.Format(Resources.StatusMileageCount, value);
        }

        public int TechPartCount
        {
            set => View.GetLabel(StatusLabel.TechPartCount).Text = string.Format(Resources.StatusTechPartCount, value);
        }
    }
}
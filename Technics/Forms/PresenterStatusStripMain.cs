using Newtonsoft.Json.Linq;
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

        private int mileageCount = 0;

        public int MileageCount
        {
            get => mileageCount;
            set
            {
                mileageCount = value;

                UpdateMileageCounts();
            }
        }

        private int mileageSelectedCount = 0;

        public int MileageSelectedCount
        {
            get => mileageSelectedCount;
            set
            {
                mileageSelectedCount = value;

                UpdateMileageCounts();
            }
        }

        private void UpdateMileageCounts()
        {
            var text = string.Format(Resources.StatusMileageCount, mileageCount);

            if (mileageSelectedCount > 1)
            {
                text += " " +
                    string.Format(Resources.StatusMileageSelectedCount, mileageSelectedCount);
            }

            View.GetLabel(StatusLabel.MileageCount).Text = text;
        }

        private int techPartCount = 0;

        public int TechPartCount
        {
            get => techPartCount;
            set
            {
                techPartCount = value;

                UpdateTechPartCounts();
            }
        }

        private int techPartSelectedCount = 0;

        public int TechPartSelectedCount
        {
            get => techPartSelectedCount;
            set
            {
                techPartSelectedCount = value;

                UpdateTechPartCounts();
            }
        }

        private void UpdateTechPartCounts()
        {
            var text = string.Format(Resources.StatusTechPartCount, techPartCount);

            if (techPartSelectedCount > 1)
            {
                text += " " +
                    string.Format(Resources.StatusTechPartSelectedCount, techPartSelectedCount);
            }

            View.GetLabel(StatusLabel.TechPartCount).Text = text;
        }
    }
}
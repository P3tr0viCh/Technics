using P3tr0viCh.Utils;
using static Technics.Enums;

namespace Technics
{
    internal class PresenterStatusStripMain : PresenterStatusStrip<PresenterStatusStripMain.StatusLabel>
    {
        public enum StatusLabel
        {
            Status,
        }

        public PresenterStatusStripMain(IPresenterStatusStrip view) : base(view)
        {
            Status = Status.Idle;
        }

        public Status Status
        {
            set => View.GetLabel(StatusLabel.Status).Text = value.Description();
        }
    }
}
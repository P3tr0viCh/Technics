using P3tr0viCh.Utils;
using Technics.Properties;

namespace Technics
{
    internal class PresenterStatusStripList : PresenterStatusStrip<PresenterStatusStripList.StatusLabel>
    {
        public enum StatusLabel
        {
            Count,
            SelectedCount,
        }

        public PresenterStatusStripList(IPresenterStatusStrip view) : base(view)
        {
            Count = 0;
            SelectedCount = 0;
        }

        public int Count
        {
            set => View.GetLabel(StatusLabel.Count).Text = string.Format(Resources.StatusCount, value);
        }

        public int SelectedCount
        {
            set
            {
                View.GetLabel(StatusLabel.SelectedCount).Text =
                    value > 1 ? string.Format(Resources.StatusSelectedCount, value) : string.Empty;
            }
        }
    }
}
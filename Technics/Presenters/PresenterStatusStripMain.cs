using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Presenters;
using Technics.Properties;
using static Technics.ProgramStatus;

namespace Technics.Presenters
{
    internal partial class PresenterStatusStripMain : PresenterStatusStrip<PresenterStatusStripMain.StatusLabel>
    {
        public enum StatusLabel
        {
            Status,
            MileagesCount,
            TechPartsCount,
            MaintenanceCount,
        }

        public PresenterStatusStripMain(IPresenterStatusStrip view) : base(view)
        {
            Mileages = new CountModel(this, StatusLabel.MileagesCount,
                Resources.StatusMileagesCount, Resources.StatusMileagesSelectedCount);
            TechParts = new CountModel(this, StatusLabel.TechPartsCount,
                Resources.StatusTechPartsCount, Resources.StatusTechPartsSelectedCount);
            Maintenance = new CountModel(this, StatusLabel.MaintenanceCount,
                Resources.StatusMaintenanceCount, Resources.StatusMaintenanceSelectedCount);

            Status = Status.Idle;

            Mileages.Count = 0;
            TechParts.Count = 0;
            Maintenance.Count = 0;
        }

        public Status Status
        {
            set => View.GetLabel(StatusLabel.Status).Text = value.Description();
        }

        public readonly CountModel Mileages;
        public readonly CountModel TechParts;
        public readonly CountModel Maintenance;
    }
}
using System.Windows.Forms;

namespace Technics
{
    internal static class DataGridViewCellStyles
    {
        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
        {
        };

        private class TopRight : DataGridViewCellStyle
        {
            public TopRight()
            {
                Alignment = DataGridViewContentAlignment.TopRight;
            }
        }

        private class TopCenter : DataGridViewCellStyle
        {
            public TopCenter()
            {
                Alignment = DataGridViewContentAlignment.TopCenter;
            }
        }

        public static readonly DataGridViewCellStyle MileagesMileage = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle MileagesMileageCommon = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle TechPartsMileage = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle TechPartsMileageCommon = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle PartsState = new TopCenter()
        {
        };

        public static readonly DataGridViewCellStyle MaintenanceMileageCommon = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle MaintenanceMileageAfterMaintenance = new TopRight()
        {
        };

        public static void UpdateSettings()
        {
            DateTime.Format = AppSettings.Settings.FormatDateTime;

            MileagesMileage.Format = AppSettings.Settings.FormatMileagesMileage;
            MileagesMileageCommon.Format = AppSettings.Settings.FormatMileagesMileageCommon;

            TechPartsMileage.Format = AppSettings.Settings.FormatTechPartsMileage;
            TechPartsMileageCommon.Format = AppSettings.Settings.FormatTechPartsMileageCommon;

            MaintenanceMileageCommon.Format = AppSettings.Settings.FormatMaintenanceMileageCommon;
            MaintenanceMileageAfterMaintenance.Format = AppSettings.Settings.FormatMaintenanceMileageAfterMaintenance;
        }
    }
}
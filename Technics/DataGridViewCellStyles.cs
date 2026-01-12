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

        public static void UpdateSettings()
        {
            DateTime.Format = AppSettings.Default.FormatDateTime;

            MileagesMileage.Format = AppSettings.Default.FormatMileagesMileage;
            MileagesMileageCommon.Format = AppSettings.Default.FormatMileagesMileageCommon;

            TechPartsMileage.Format = AppSettings.Default.FormatTechPartsMileage;
            TechPartsMileageCommon.Format = AppSettings.Default.FormatTechPartsMileageCommon;
        }
    }
}
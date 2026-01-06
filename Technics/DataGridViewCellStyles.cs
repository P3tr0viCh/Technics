using System.Windows.Forms;

namespace Technics
{
    internal static class DataGridViewCellStyles
    {
        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
        {
        };

        public static readonly DataGridViewCellStyle Mileage = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight
        };

        public static void UpdateSettings()
        {
            DateTime.Format = AppSettings.Default.FormatDateTime;
            Mileage.Format = AppSettings.Default.FormatMileage;
        }
    }
}
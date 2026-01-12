using System.Threading.Tasks;

namespace Technics
{
    public partial class Main
    {
        public void UpdateSettings()
        {
            DataGridViewCellStyles.UpdateSettings();

            MileagesDateTime.DefaultCellStyle =
            TechPartsDateTimeInstall.DefaultCellStyle =
            TechPartsDateTimeRemove.DefaultCellStyle =
                DataGridViewCellStyles.DateTime;

            MileagesMileage.DefaultCellStyle = DataGridViewCellStyles.MileagesMileage;
            MileagesMileageCommon.DefaultCellStyle = DataGridViewCellStyles.MileagesMileageCommon;
            TechPartsMileage.DefaultCellStyle = DataGridViewCellStyles.TechPartsMileage;
            TechPartsMileageCommon.DefaultCellStyle = DataGridViewCellStyles.TechPartsMileageCommon;
        }

        private async Task ShowSettingsAsync()
        {
            var frmSettings = new FrmSettings(AppSettings.Default);

            if (frmSettings.ShowDialog(this))
            {
                if (!SetDatabase()) return;

                UpdateSettings();

                await UpdateDataAsync();
            }
        }
    }
}
using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using Technics.Properties;

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

            MileagesMileage.DefaultCellStyle =
            MileagesMileageCommon.DefaultCellStyle =
            TechPartsMileage.DefaultCellStyle =
            TechPartsMileageCommon.DefaultCellStyle =
                DataGridViewCellStyles.Mileage;
        }

        private async Task ShowSettingsAsync()
        {
            var options = new FrmSettings.Options
            {
                BeforeOpen = FrmSettingsBeforeOpen,
                AfterClose = FrmSettingsAfterClose,

                LoadFormState = FrmSettingsLoadFormState,
                SaveFormState = FrmSettingsSaveFormState,

                CheckSettings = FrmSettingsCheckSettings
            };

            if (FrmSettings.Show(this, AppSettings.Default, options))
            {
                if (!SetDatabase()) return;

                UpdateSettings();

                await UpdateDataAsync();
            }
        }

        private void AssertDirectory(string path)
        {
            if (path.IsEmpty()) return;

            if (Directory.Exists(path)) return;

            throw new Exceptions.DirectoryNotExistsException(
                Resources.ErrorDirectoryNotExists, path);
        }

        private void AssertDirectories()
        {
            AssertDirectory(AppSettings.Default.DirectoryDatabase);
        }

        private bool FrmSettingsCheckSettings(FrmSettings frm)
        {
            try
            {
                AssertDirectories();

                return true;
            }
            catch (Exception e)
            {
                Utils.Log.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private void FrmSettingsSaveFormState(FrmSettings frm)
        {
            AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(frm);
        }

        private void FrmSettingsLoadFormState(FrmSettings frm)
        {
            AppSettings.LoadFormState(frm, AppSettings.Default.FormStateSettings);
        }

        private void FrmSettingsBeforeOpen(FrmSettings frm)
        {
            Utils.Log.WriteFormOpen(frm);
        }

        private void FrmSettingsAfterClose(FrmSettings frm)
        {
            Utils.Log.WriteFormClose(frm);
        }
    }
}
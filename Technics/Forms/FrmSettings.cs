using P3tr0viCh.Utils;
using System;
using System.IO;
using Technics.Properties;

namespace Technics
{
    internal class FrmSettings : FrmSettingsBase
    {
        public FrmSettings(ISettingsBase settings) : base(settings)
        {
        }

        protected override void BeforeOpen()
        {
            Utils.Log.WriteFormOpen(this);
        }

        protected override void AfterClose()
        {
            Utils.Log.WriteFormClose(this);
        }

        protected override void SaveFormState()
        {
            AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(this);
        }

        protected override void LoadFormState()
        {
            AppSettings.LoadFormState(this, AppSettings.Default.FormStateSettings);
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

        protected override bool CheckSettings()
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
    }
}
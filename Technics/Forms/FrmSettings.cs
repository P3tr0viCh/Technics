using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Forms;
using System;

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

        protected override void SettingsHasError(Exception e)
        {
            Utils.Log.Error(e);

            Utils.Msg.Error(e.Message);
        }
    }
}
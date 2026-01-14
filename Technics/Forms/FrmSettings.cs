using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Settings;
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
            AppSettings.SaveFormState(this, AppSettings.Default.FormStates);
        }

        protected override void LoadFormState()
        {
            AppSettings.LoadFormState(this, AppSettings.Default.FormStates);
        }

        protected override void SettingsHasError(Exception e)
        {
            Utils.Log.Error(e);

            Utils.Msg.Error(e.Message);
        }
    }
}
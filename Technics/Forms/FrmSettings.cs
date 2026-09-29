using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Storage;
using System;

namespace Technics
{
    internal class FrmSettings : FrmSettingsBase
    {
        public FrmSettings(IObjectStorage objectStorage) : base(objectStorage)
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
            AppSettings.Default.SaveFormState(this);
        }

        protected override void LoadFormState()
        {
            AppSettings.Default.LoadFormState(this);
        }

        protected override void SettingsHasError(Exception e)
        {
            Utils.Log.Error(e);

            Utils.Msg.Error(e.Message);
        }
    }
}
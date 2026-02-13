using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Interfaces;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Technics.Interfaces;
using Technics.Properties;

namespace Technics.Presenters
{
    internal abstract class PresenterFrmList<T> : PresenterFrmListBase<T>, IPresenterFrmList where T : BaseId, new()
    {
        public abstract FrmListType ListType { get; }

        public PresenterFrmList(IFrmList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiDelete);

            FormOpened += PresenterFrmList_FormOpened;
            FormClosed += PresenterFrmList_FormClosed;

            StatusStart += PresenterFrmList_StatusStart;
            StatusStop += PresenterFrmList_StatusStop;

            ItemsExceptionLoad += (sender, e) => PresenterFrmList_Exception(Resources.MsgDatabaseLoadFail, e.Exception);
            ItemsExceptionChange += (sender, e) => PresenterFrmList_Exception(Resources.MsgDatabaseListItemSaveFail, e.Exception);
            ItemsExceptionDelete += (sender, e) => PresenterFrmList_Exception(Resources.MsgDatabaseListItemDeleteFail, e.Exception);
        }

        private void FormOpen()
        {
            Utils.Log.WriteFormOpen(Form);

            Utils.Log.Info($"{ListType}");
        }

        private void FormClose()
        {
            Utils.Log.Info($"{ListType}");

            Utils.Log.WriteFormClose(Form);
        }

        private void PresenterFrmList_FormOpened(object sender, EventArgs e)
        {
            FormOpen();
        }

        private void PresenterFrmList_FormClosed(object sender, EventArgs e)
        {
            FormClose();
        }

        protected override void LoadFormState()
        {
            FrmList.ToolStrip.SetShowTextAndToolTips(AppSettings.Default.ToolStripsShowText);

            AppSettings.LoadFormState(Form, ListType.ToString(), AppSettings.Default.FormStates);
            AppSettings.LoadDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Default.ColumnStates);
        }

        protected override void SaveFormState()
        {
            AppSettings.SaveFormState(Form, ListType.ToString(), AppSettings.Default.FormStates);
            AppSettings.SaveDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Default.ColumnStates);

            AppSettings.Default.Save();
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(BaseId.Id)].Visible = false;
            FrmList.DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
        }

        private void PresenterFrmList_StatusStart(object sender, StatusEventArgs e)
        {
            var status = ProgramStatus.Status.Idle;

            switch (e.Status)
            {
                case FrmListDatabaseActionStatus.Load:
                    status = ProgramStatus.Status.LoadData;
                    break;
                case FrmListDatabaseActionStatus.Save:
                case FrmListDatabaseActionStatus.Delete:
                    status = ProgramStatus.Status.SaveData;
                    break;
            }

            e.Object = ProgramStatus.Default.Start(status);
        }

        private void PresenterFrmList_StatusStop(object sender, StatusEventArgs e)
        {
            ProgramStatus.Default.Stop((ProgramStatus<ProgramStatus.Status>.Status)e.Object);
        }

        private void PresenterFrmList_Exception(string message, Exception e)
        {
            Utils.Log.Query(e);

            Utils.Log.Error(e);

            Utils.Msg.Error(message, e.Message);
        }

        protected override async Task<IEnumerable<T>> DatabaseListLoadAsync(CancellationToken cancellationToken)
        {
            return await Database.Default.ListLoadAsync<T>();
        }

        protected override async Task DatabaseListItemsSaveAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemSaveAsync(list);
        }

        protected override async Task DatabaseListItemsDeleteAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemDeleteAsync(list);
        }
    }
}
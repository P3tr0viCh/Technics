using P3tr0viCh.Database;
using P3tr0viCh.Utils.Interfaces;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.ProgramStatus;

namespace Technics.Presenters
{
    internal abstract class PresenterFrmListBase<T> : PresenterFrmList<T> where T : BaseId, new()
    {
        public abstract FrmListType ListType { get; }

        public PresenterFrmListBase(IFrmList frmList) : base(frmList)
        {
        }

        protected override void FormOpened()
        {
            Utils.Log.WriteFormOpen(Form);

            Utils.Log.Info($"ListType = {ListType}");
        }

        protected override void FormClosed()
        {
            Utils.Log.WriteFormClose(Form);
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

        protected override object StatusStartLoad()
        {
            return ProgramStatus.Default.Start(Status.LoadData);
        }

        protected override object StatusStartSave()
        {
            return ProgramStatus.Default.Start(Status.SaveData);
        }

        protected override object StatusStartDelete()
        {
            return ProgramStatus.Default.Start(Status.SaveData);
        }

        protected override void StatusStop(object status)
        {
            ProgramStatus.Default.Stop((P3tr0viCh.Utils.ProgramStatus<Status>.Status)status);
        }

        protected override async Task<IEnumerable<T>> ListLoadAsync(CancellationToken cancellationToken)
        {
            return await Database.Default.ListLoadAsync<T>();
        }

        protected override async Task ListItemSaveAsync(T value)
        {
            await Database.Default.ListItemSaveAsync(value);
        }

        protected override async Task ListItemSaveAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemSaveAsync(list);
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemDeleteAsync(list);
        }

        private void PerformListLoadException(Exception e, string message)
        {
            Utils.Log.Query(e);

            Utils.Log.Error(e);

            Utils.Msg.Error(message, e.Message);
        }

        protected override void ListLoadException(Exception e)
        {
            PerformListLoadException(e, Resources.MsgDatabaseLoadFail);
        }

        protected override void ListItemChangeException(Exception e)
        {
            PerformListLoadException(e, Resources.MsgDatabaseListItemSaveFail);
        }

        protected override void ListItemDeleteException(Exception e)
        {
            PerformListLoadException(e, Resources.MsgDatabaseListItemDeleteFail);
        }
    }
}
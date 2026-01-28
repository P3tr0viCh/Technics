using P3tr0viCh.Database;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Interfaces;
using Technics.Properties;

namespace Technics.Presenters
{
    internal abstract partial class PresenterFrmListBase<T> :
        IPresenterFrmList,
        IPresenterDataGridViewCompare<T> where T : BaseId, new()
    {
        public IFrmList FrmList { get; private set; }

        public Form Form => FrmList as Form;

        public abstract FrmListType ListType { get; }

        public bool Changed { get; set; } = false;

        public DataGridView DataGridView => FrmList.DataGridView;

        internal readonly BindingSource bindingSource;

        internal readonly PresenterDataGridViewFrmList<T> presenterDataGridView;

        public PresenterFrmListBase(IFrmList frmList)
        {
            FrmList = frmList;

            Form.Load += new EventHandler(Form_Load);
            Form.FormClosing += new FormClosingEventHandler(FrmList_FormClosing);

            bindingSource = new BindingSource();

            presenterDataGridView = new PresenterDataGridViewFrmList<T>(this);

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            await FormLoadAsync();
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosing();
        }

        private FrmListGrant grants = FrmListGrant.All;
        protected FrmListGrant Grants
        {
            get => grants;
            set
            {
                grants = value;

                foreach (ToolStripItem item in FrmList.ToolStrip.Items)
                {
                    if (item.Name == "tsbtnClose")
                    {
                        continue;
                    }

                    if (item.Name == "tsbtnAdd")
                    {
                        item.Visible = CanAdd;
                        continue;
                    }

                    if (item.Name == "tsbtnChange")
                    {
                        item.Visible = CanChange;
                        continue;
                    }

                    if (item.Name == "tsbtnDelete")
                    {
                        item.Visible = CanDelete;
                        continue;
                    }

                    if (item.Name == "toolStripSeparator1")
                    {
                        item.Visible = CanAdd || CanChange || CanDelete;
                        continue;
                    }
                }
            }
        }

        private bool CanAdd => grants.HasFlag(FrmListGrant.Add);
        private bool CanChange => grants.HasFlag(FrmListGrant.Change);
        private bool CanDelete => grants.HasFlag(FrmListGrant.Delete);

        private void SetDataSource()
        {
            bindingSource.DataSource = Enumerable.Empty<T>();

            DataGridView.DataSource = bindingSource;
        }

        protected abstract string FormTitle { get; }

        protected virtual void LoadFormState()
        {
            AppSettings.LoadFormState(Form, ListType.ToString(), AppSettings.Default.FormStates);
            AppSettings.LoadDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Default.ColumnStates);
        }

        protected virtual void SaveFormState()
        {
            AppSettings.SaveFormState(Form, ListType.ToString(), AppSettings.Default.FormStates);
            AppSettings.SaveDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Default.ColumnStates);
        }

        protected abstract void UpdateColumns();

        private async Task FormLoadAsync()
        {
            Utils.Log.WriteFormOpen(Form);

            Utils.Log.Info($"ListType = {ListType}");

            Form.Text = FormTitle;

            FrmList.ToolStrip.SetShowTextAndToolTips(AppSettings.Default.ToolStripsShowText);

            DataGridView.MultiSelect = true;

            SetDataSource();

            LoadFormState();

            UpdateCommonColumns();

            UpdateColumns();

            await ListLoadAsync();
        }

        private void UpdateCommonColumns()
        {
            DataGridView.Columns[nameof(BaseId.Id)].Visible = false;
            DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
        }

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.Default.Save();

            Utils.Log.WriteFormClose(Form);
        }

        public T Find(T value)
        {
            return bindingSource.Cast<T>().Where(item => item.Id == value.Id).FirstOrDefault();
        }

        public T Selected
        {
            get => presenterDataGridView.Selected;
            set => presenterDataGridView.Selected = Find(value);
        }

        public IEnumerable<T> SelectedList
        {
            get => DataGridView.GetSelectedList<T>();
        }

        public event ListChanged OnListChanged;

        private void PerformOnListChanged()
        {
            Changed = true;

            OnListChanged?.Invoke();
        }

        private void ListItemChange(T value)
        {
            var item = Find(value);

            if (item == default)
            {
                bindingSource.Add(value);
            }
            else
            {
                var index = bindingSource.IndexOf(item);

                bindingSource.List[index] = value;

                bindingSource.ResetItem(index);
            }

            DataGridView.SetSelectedRows(value);

            presenterDataGridView.Sort();

            PerformOnListChanged();
        }

        private void ListItemDelete(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                bindingSource.Remove(item);
            }

            PerformOnListChanged();
        }

        protected virtual T GetNewItem() => new T();

        protected abstract bool ShowItemChangeDialog(T value);
        protected abstract bool ShowItemDeleteDialog(IEnumerable<T> list);

        private async Task ListItemChangeAsync(T value)
        {
            try
            {
                if (!ShowItemChangeDialog(value)) return;

                await PerformListItemSaveAsync(value);

                ListItemChange(value);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
            }
        }

        public async Task ListItemAddNewAsync()
        {
            if (!CanAdd) return;

            var item = GetNewItem();

            await ListItemChangeAsync(item);
        }

        public async Task ListItemChangeSelectedAsync()
        {
            if (!CanChange) return;

            var item = Selected;

            DataGridView.SetSelectedRows(item);

            await ListItemChangeAsync(item);
        }

        public async Task ListItemDeleteSelectedAsync()
        {
            if (!CanDelete) return;

            try
            {
                var list = SelectedList;

                DataGridView.SetSelectedRows(list);

                if (!ShowItemDeleteDialog(list)) return;

                await PerformListItemDeleteAsync(list);

                ListItemDelete(list);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemDeleteFail, e.Message);
            }
        }

        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await ListItemChangeSelectedAsync();
        }

        public abstract int Compare(T x, T y, string dataPropertyName, ComparerSortOrder sortOrder);
    }
}
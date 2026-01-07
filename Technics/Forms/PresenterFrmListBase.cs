using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technics
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
        public BindingSource BindingSource => FrmList.BindingSource;

        internal readonly PresenterDataGridViewFrmList<T> presenterDataGridView;

        public PresenterFrmListBase(IFrmList frmList)
        {
            FrmList = frmList;

            Form.Load += new EventHandler(Form_Load);
            Form.FormClosing += new FormClosingEventHandler(FrmList_FormClosing);

            presenterDataGridView = new PresenterDataGridViewFrmList<T>(this);

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
        }

        private async void Form_Load(object sender, System.EventArgs e)
        {
            await FormLoadAsync();
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosing();
        }

        private FrmListGrant grants = FrmListGrant.Add | FrmListGrant.Change | FrmListGrant.Delete;
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
            BindingSource.DataSource = Enumerable.Empty<T>();

            DataGridView.DataSource = BindingSource;
        }

        protected abstract string FormTitle { get; }

        protected abstract void LoadFormState();
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

        protected abstract void SaveFormState();

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.Default.Load();

            Utils.Log.WriteFormClose(Form);
        }

        public T Find(T value)
        {
            return BindingSource.Cast<T>().Where(item => item.Id == value.Id).FirstOrDefault();
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
                BindingSource.Add(value);
            }
            else
            {
                var index = BindingSource.IndexOf(item);

                BindingSource.List[index] = value;

                BindingSource.ResetItem(index);
            }

            DataGridView.SetSelectedRows(value);

            presenterDataGridView.Sort();

            PerformOnListChanged();
        }

        private void ListItemDelete(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                BindingSource.Remove(item);
            }

            PerformOnListChanged();
        }

        protected virtual T GetNewItem() => new T();

        protected abstract bool ShowItemChangeDialog(T value);
        protected abstract bool ShowItemDeleteDialog(IEnumerable<T> list);

        private async Task ListItemChangeAsync(T value)
        {
            if (!ShowItemChangeDialog(value)) return;

            await PerformListItemSaveAsync(value);

            ListItemChange(value);
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

            var list = SelectedList;

            DataGridView.SetSelectedRows(list.Cast<BaseId>());

            if (!ShowItemDeleteDialog(list)) return;

            await PerformListItemDeleteAsync(list);

            ListItemDelete(list);
        }

        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await ListItemChangeSelectedAsync();
        }

        public abstract int Compare(T x, T y, string dataPropertyName);
    }
}
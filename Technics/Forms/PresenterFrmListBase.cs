using P3tr0viCh.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technics
{
    internal abstract partial class PresenterFrmListBase<T> : IPresenterFrmList where T : BaseId, new()
    {
        public IFrmList FrmList { get; private set; }

        public Form Form => FrmList as Form;

        public abstract FrmListType ListType { get; }

        public bool Changed { get; set; } = false;

        private DataGridView DataGridView => FrmList.DataGridView;
        private BindingSource BindingSource => FrmList.BindingSource;

        internal readonly PresenterDataGridViewFrmList presenterDataGridView;

        public PresenterFrmListBase(IFrmList frmList)
        {
            FrmList = frmList;

            Form.Load += new EventHandler(Form_Load);
            Form.FormClosing += new FormClosingEventHandler(FrmList_FormClosing);

            presenterDataGridView = new PresenterDataGridViewFrmList(DataGridView);

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

        public event ListChanged OnListChanged;

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
            ////DataGridView.Columns[nameof(BaseId.Id)].Visible = false;
            //DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
        }

        protected abstract void SaveFormState();

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.Save();

            Utils.Log.WriteFormClose(Form);
        }

        public int Count => BindingSource.Count;
        public int SelectedCount => DataGridView.SelectedCells
                    .Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Count();

        public T Find(T value)
        {
            return BindingSource.Cast<T>().Where(item => item.Id == value.Id).FirstOrDefault();
        }

        public T Selected
        {
            get => BindingSource.Current as T;
            set => BindingSource.Position = BindingSource.IndexOf(Find(value));
        }

        public IEnumerable<T> SelectedList
        {
            get
            {
                if (DataGridView.SelectedCells.Count == 0) return Enumerable.Empty<T>();

                var selectedRows = DataGridView.SelectedCells
                    .Cast<DataGridViewCell>()
                    .Select(cell => cell.OwningRow).Distinct();

                if (selectedRows?.Count() == 0) return Enumerable.Empty<T>();

                return selectedRows.Select(item => (T)item.DataBoundItem);
            }
            set
            {
                DataGridView.SetSelectedRows(value as BaseId);
            }
        }

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

        protected abstract T GetNewItem();

        protected abstract bool ShowItemChangeDialog(T value);
        protected abstract bool ShowItemDeleteDialog(IEnumerable<T> list);

        private async Task ListItemChangeAsync(T value)
        {
            if (!ShowItemChangeDialog(value)) return;

            await ListItemSaveAsync(value);

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

            await ListItemDeleteAsync(list);

            ListItemDelete(list);
        }
        
        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await ListItemChangeSelectedAsync();
        }
    }
}
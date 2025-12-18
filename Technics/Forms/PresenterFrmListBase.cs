using Newtonsoft.Json.Linq;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Technics.Enums;

namespace Technics
{
    internal abstract partial class PresenterFrmListBase<T> : IPresenterFrmList where T : BaseId, new()
    {
        public IFrmList FrmList { get; private set; }

        public abstract ListType ListType { get; }

        public bool Changed { get; set; } = false;

        public PresenterFrmListBase(IFrmList frmList)
        {
            FrmList = frmList;
        }

        public event ListChanged OnListChanged;

        private void SetDataSource()
        {
            FrmList.BindingSource.DataSource = typeof(T);

            FrmList.DataGridView.DataSource = FrmList.BindingSource;
        }

        protected abstract void LoadFormState();
        protected abstract void UpdateColumns();

        public async Task FormLoadAsync()
        {
            FrmList.ToolStrip.SetShowTextAndToolTips(AppSettings.Default.ToolStripsShowText);

            FrmList.DataGridView.MultiSelect = true;

            SetDataSource();

            LoadFormState();

            UpdateCommonColumns();

            UpdateColumns();

            await ListLoadAsync();
        }

        private void UpdateCommonColumns()
        {
            FrmList.DataGridView.Columns[nameof(BaseId.Id)].Visible = false;

            if (FrmList.DataGridView.ColumnExists(nameof(BaseId.IsNew)))
            {
                FrmList.DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
            }
        }

        protected abstract void SaveFormState();

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.Save();
        }
        
        public int Count => FrmList.BindingSource.Count;
        public int SelectedCount => FrmList.DataGridView.SelectedCells
                    .Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Count();

        public T Find(T value)
        {
            return FrmList.BindingSource.Cast<T>().Where(item => item.Id == value.Id).FirstOrDefault();
        }

        public T Selected
        {
            get => ((BindingSource)FrmList.DataGridView.DataSource).Current as T;
            set => FrmList.BindingSource.Position = FrmList.BindingSource.IndexOf(Find(value));
        }

        public List<T> SelectedList
        {
            get
            {
                if (FrmList.DataGridView.SelectedCells.Count == 0) return default;

                var selectedRows = FrmList.DataGridView.SelectedCells
                    .Cast<DataGridViewCell>()
                    .Select(cell => cell.OwningRow).Distinct();

                if (selectedRows?.Count() == 0) return null;

                return selectedRows.Select(item => (T)item.DataBoundItem).ToList();
            }
            set
            {
                SetSelectedRows(value);
            }
        }

        private void SetSelectedRows(List<T> values)
        {
            FrmList.DataGridView.ClearSelection();

            foreach (var value in values)
            {
                foreach (var row in from DataGridViewRow row in FrmList.DataGridView.Rows
                                    where (row.DataBoundItem as BaseId).Id == value.Id
                                    select row)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private void SetSelectedRows(T value)
        {
            SetSelectedRows(new List<T>() { value });
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
                FrmList.BindingSource.Add(value);
            }
            else
            {
                var index = FrmList.BindingSource.IndexOf(item);

                FrmList.BindingSource.List[index] = value;

                FrmList.BindingSource.ResetItem(index);
            }

            SetSelectedRows(value);

            PerformOnListChanged();

            //            Sort();
        }
        
        private void ListItemDelete(List<T> list)
        {
            list.ForEach(item => FrmList.BindingSource.Remove(item));

            PerformOnListChanged();
        }

        protected abstract T GetNewItem();

        protected abstract bool ShowItemChangeDialog(T value);
        protected abstract bool ShowItemDeleteDialog(List<T> list);

        public async Task ListItemChangeAsync(T value)
        {
            if (!ShowItemChangeDialog(value)) return;

            await ListItemSaveAsync(value);

            ListItemChange(value);
        }

        public async Task ListItemAddNewAsync()
        {
            var item = GetNewItem();

            await ListItemChangeAsync(item);
        }

        public async Task ListItemChangeSelectedAsync()
        {
            var item = Selected;

            SetSelectedRows(item);

            await ListItemChangeAsync(item);
        }

        public async Task ListItemDeleteSelectedAsync()
        {
            var list = SelectedList;

            SetSelectedRows(list);

            if (!ShowItemDeleteDialog(list)) return;

            await ListItemDeleteAsync(list);

            ListItemDelete(list);
        }
    }
}
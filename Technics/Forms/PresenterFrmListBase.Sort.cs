using P3tr0viCh.Utils;
using System.Linq;

namespace Technics
{
    internal abstract partial class PresenterFrmListBase<T>
    {
        protected abstract int Compare(T x, T y);

        private string sortColumn = string.Empty;
        protected string SortColumn
        {
            get => sortColumn;
            set
            {
                if (sortColumn == value)
                {
                    SortOrderDescending = !SortOrderDescending;
                    
                    return;
                }

                sortColumn = value;
            }
        }

        protected bool SortOrderDescending { get; set; } = false;

        private void Sort()
        {
            if (Count == 0) return;

            if (SortColumn.IsEmpty()) return;

            var selected = Selected;

            var list = BindingSource.Cast<T>().ToList();

            list.Sort((T x, T y) =>
            {
                var compare = Compare(x, y);

                if (SortOrderDescending)
                {
                    compare = -compare;
                }

                return compare;
            });

            BindingSource.DataSource = list;

            Selected = selected;
        }

        protected string GetSortColumn(int columnIndex)
        {
            return DataGridView.Columns[columnIndex].DataPropertyName;
        }
    }
}
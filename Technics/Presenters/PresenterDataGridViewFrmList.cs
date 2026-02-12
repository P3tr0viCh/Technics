using P3tr0viCh.Database;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Presenters;

namespace Technics.Presenters
{
    internal class PresenterDataGridViewFrmList<T> : PresenterDataGridView<T> where T : BaseId, new()
    {
        private readonly PresenterFrmList<T> presenterFrmList;

        public PresenterDataGridViewFrmList(PresenterFrmList<T> presenterFrmList) : 
            base(presenterFrmList.FrmList.DataGridView)
        {
            this.presenterFrmList = presenterFrmList;
        }

        public override int Compare(T x, T y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return presenterFrmList.Compare(x, y, dataPropertyName, sortOrder);    
        }
    }
}
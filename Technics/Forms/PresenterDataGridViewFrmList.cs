using P3tr0viCh.Database;
using P3tr0viCh.Utils;

namespace Technics
{
    internal class PresenterDataGridViewFrmList<T> : PresenterDataGridView<T> where T : BaseId, new()
    {
        private readonly PresenterFrmListBase<T> presenterFrmList;

        public PresenterDataGridViewFrmList(PresenterFrmListBase<T> presenterFrmList) : 
            base(presenterFrmList.FrmList.DataGridView)
        {
            this.presenterFrmList = presenterFrmList;
        }

        public override int Compare(T x, T y, string dataPropertyName)
        {
            return presenterFrmList.Compare(x, y, dataPropertyName);    
        }
    }
}
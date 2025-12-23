using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technics
{
    public delegate void ListChanged();

    internal interface IPresenterFrmList
    {
        FrmListType ListType { get; }

        IFrmList FrmList { get; }

        bool Changed { get; }

        int Count { get; }
        int SelectedCount { get; }

        event ListChanged OnListChanged;

        Task ListItemAddNewAsync();

        Task ListItemChangeSelectedAsync();

        Task ListItemDeleteSelectedAsync();
    }
}
using System.Threading.Tasks;

namespace Technics.Interfaces
{
    public delegate void ListChanged();

    internal interface IPresenterFrmList
    {
        IFrmList FrmList { get; }

        FrmListType ListType { get; }

        event ListChanged OnListChanged;

        bool Changed { get; }

        Task ListItemAddNewAsync();

        Task ListItemChangeSelectedAsync();

        Task ListItemDeleteSelectedAsync();
    }
}
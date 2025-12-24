using System.Threading.Tasks;

namespace Technics
{
    public delegate void ListChanged();

    internal interface IPresenterFrmList
    {
        FrmListType ListType { get; }

        IFrmList FrmList { get; }


        event ListChanged OnListChanged;

        bool Changed { get; }

        Task ListItemAddNewAsync();

        Task ListItemChangeSelectedAsync();

        Task ListItemDeleteSelectedAsync();
    }
}
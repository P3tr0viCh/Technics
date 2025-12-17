using System.Collections.Generic;
using System.Threading.Tasks;
using static Technics.Enums;

namespace Technics
{
    public delegate void ListChanged();

    internal interface IPresenterFrmList
    {
        ListType ListType { get; }

        IFrmList FrmList { get; }

        bool Changed { get; }

        int Count { get; }
        int SelectedCount { get; }

        Task FormLoadAsync();

        void FormClosing();

        event ListChanged OnListChanged;

        Task ListItemAddNewAsync();

        Task ListItemDeleteSelectedAsync();
    }
}
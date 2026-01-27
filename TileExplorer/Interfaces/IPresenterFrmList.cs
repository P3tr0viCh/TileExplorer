using P3tr0viCh.Utils;
using System.Threading.Tasks;

namespace TileExplorer.Interfaces
{
    public delegate void ListChanged();

    internal interface IPresenterFrmList : IUpdateDataForm
    {
        FrmListType ListType { get; }

        IFrmList FrmList { get; }

        object Value { get; set; }

        event ListChanged OnListChanged;

        bool Changed { get; }

        Task ListItemAddNewAsync();

        Task ListItemChangeSelectedAsync();

        Task ListItemDeleteSelectedAsync();

        IBaseId Find(IBaseId value);

        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IPresenterFrmListBase : IPresenterFrmList, IChildForm, IFrmUpdateData
    {
        object Value { get; set; }

        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
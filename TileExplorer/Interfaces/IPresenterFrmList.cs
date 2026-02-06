using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    public delegate void PositionChanged();

    internal interface IPresenterFrmListBase : IUpdateDataForm, IPresenterFrmList
    {
        FrmListType ListType { get; }

        object Value { get; set; }

//        event PositionChanged OnPositionChanged;

        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
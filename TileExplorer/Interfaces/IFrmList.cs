using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IFrmListBase2 : IChildForm, IFrmUpdateData, IFrmList
    {
        object Value { get; }

        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IFrmListBase: IUpdateDataForm, IFrmList
    {
        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
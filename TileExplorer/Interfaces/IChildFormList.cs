using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    public interface IChildFormList : IFrmList
    {
        bool Loading { set; }
    }
}
using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IPresenterFrmList : IPresenterFrmListBase, IChildForm
    {
        object Value { get; set; }
    }
}
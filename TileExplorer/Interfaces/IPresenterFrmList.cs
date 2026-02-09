using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IPresenterFrmListBase : IPresenterFrmList, IChildForm, IFrmUpdateData
    {
        object Value { get; set; }
    }
}
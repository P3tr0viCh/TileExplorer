using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    internal interface IPresenterFrmList : IPresenterFrmListBase, IChildForm,
        IFrmUpdateSettings, IFrmUpdateData, IFrmUpdateDataList
    {
        object Value { get; set; }
    }
}
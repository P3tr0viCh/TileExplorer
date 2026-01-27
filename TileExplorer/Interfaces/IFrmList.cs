using P3tr0viCh.Utils;
using System.Windows.Forms;

namespace TileExplorer.Interfaces
{
    internal interface IFrmList: IUpdateDataForm
    {
        DataGridView DataGridView { get; }

        ToolStrip ToolStrip { get; }

        StatusStrip StatusStrip { get; }

        void ListItemChange(IBaseId value);

        void ListItemDelete(IBaseId value);
    }
}
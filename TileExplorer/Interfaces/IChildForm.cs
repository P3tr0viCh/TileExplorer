using P3tr0viCh.Utils.Interfaces;

namespace TileExplorer.Interfaces
{
    public interface IChildForm: IFrmUpdateSettings
    {
        IMainForm MainForm { get; }

        ChildFormType FormType { get; }
    }
}
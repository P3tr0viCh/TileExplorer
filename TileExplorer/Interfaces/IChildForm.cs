namespace TileExplorer.Interfaces
{
    public interface IChildForm
    {
        IMainForm MainForm { get; }

        ChildFormType FormType { get; }

        void UpdateSettings();
    }
}
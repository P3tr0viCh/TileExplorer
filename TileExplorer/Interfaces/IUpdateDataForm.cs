using System.Threading.Tasks;

namespace TileExplorer.Interfaces
{
    public interface IUpdateDataForm : IChildForm
    {
        Task UpdateDataAsync();
    }
}
using P3tr0viCh.Utils;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private void EquipmentAdd(Equipment equipment)
        {
            if (!FrmEquipment.ShowDlg(this, equipment)) return;

            Utils.Forms.GetFrmList(ChildFormType.EquipmentList)?.ListItemChange(equipment);
        }

        private async Task EquipmentChangeAsync(Equipment equipment)
        {
            if (!FrmEquipment.ShowDlg(this, equipment)) return;
            
            await UpdateDataAsync(DataLoad.ObjectChange, equipment);
        }

        private async Task EquipmentDeleteAsync(Equipment equipment)
        {
            var name = equipment.Name;

            if (!Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name))) return;

            if (!await Database.Actions.EquipmentDeleteAsync(equipment)) return;

            await UpdateDataAsync(DataLoad.ObjectDelete, equipment);
        }
    }
}
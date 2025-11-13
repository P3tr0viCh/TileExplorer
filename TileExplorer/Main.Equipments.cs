using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
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

        private async Task EquipmentDeleteAsync(List<Equipment> equipments)
        {
            if (equipments?.Count == 0) return;

            var firstEquipment = equipments.FirstOrDefault();

            var name = firstEquipment.Name;

            var question = equipments.Count == 1 ? Resources.QuestionEquipmentDelete : Resources.QuestionEquipmentsDelete;

            if (!Msg.Question(question, name, equipments.Count - 1)) return;

            if (!await Database.Actions.EquipmentDeleteAsync(equipments)) return;

            foreach (var equipment in equipments)
            {
                await UpdateDataAsync(DataLoad.ObjectDelete, equipment);
            }
        }
    }
}
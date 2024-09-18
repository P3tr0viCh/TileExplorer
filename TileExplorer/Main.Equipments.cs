using P3tr0viCh.Utils;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private void EquipmentAdd(Equipment equipment)
        {
            if (FrmEquipment.ShowDlg(this, equipment))
            {
                Utils.GetFrmList(ChildFormType.EquipmentList)?.ListItemChange(equipment);
            }
        }

        private void EquipmentChange(Equipment equipment)
        {
            if (FrmEquipment.ShowDlg(this, equipment))
            {
                Utils.GetFrmList(ChildFormType.EquipmentList)?.ListItemChange(equipment);

                Utils.GetFrmList(ChildFormType.TrackList)?.UpdateData();

                Utils.GetFrmList(ChildFormType.ResultEquipments)?.UpdateData();
            }
        }

        private void EquipmentDelete(Equipment equipment)
        {
            var name = equipment.Name;

            if (!Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name))) return;

            if (Database.Actions.EquipmentDeleteAsync(equipment).Result)
            {
                Utils.GetFrmList(ChildFormType.EquipmentList)?.ListItemDelete(equipment);

                Utils.GetFrmList(ChildFormType.TrackList)?.UpdateData();

                Utils.GetFrmList(ChildFormType.ResultEquipments)?.UpdateData();
            }
        }
    }
}
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
            if (!FrmEquipment.ShowDlg(this, equipment)) return;

            Utils.GetFrmList(ChildFormType.EquipmentList)?.ListItemChange(equipment);
        }

        private void EquipmentChange(Equipment equipment)
        {
            if (!FrmEquipment.ShowDlg(this, equipment)) return;

            Utils.GetChildForms<FrmList>(null).ForEach(frm =>
            {
                switch (frm.FormType)
                {
                    case ChildFormType.EquipmentList:
                        frm.ListItemChange(equipment);
                        break;
                    case ChildFormType.TrackList:
                        frm.UpdateData();
                        break;
                    case ChildFormType.ResultEquipments:
                        frm.UpdateData();
                        break;
                }
            });
        }

        private void EquipmentDelete(Equipment equipment)
        {
            var name = equipment.Name;

            if (!Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name))) return;

            if (!Database.Actions.EquipmentDeleteAsync(equipment).Result) return;

            Utils.GetChildForms<FrmList>(null).ForEach(frm =>
            {
                switch (frm.FormType)
                {
                    case ChildFormType.EquipmentList:
                        frm.ListItemDelete(equipment);
                        break;
                    case ChildFormType.TrackList:
                        frm.UpdateData();
                        break;
                    case ChildFormType.ResultEquipments:
                        frm.UpdateData();
                        break;
                }
            });
        }
    }
}
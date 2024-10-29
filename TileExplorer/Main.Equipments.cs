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

        private void EquipmentChange(Equipment equipment)
        {
            if (!FrmEquipment.ShowDlg(this, equipment)) return;

            Utils.Forms.GetChildForms<FrmList>().ForEach(async frm =>
            {
                switch (frm.FormType)
                {
                    case ChildFormType.EquipmentList:
                        frm.ListItemChange(equipment);
                        break;
                    case ChildFormType.TrackList:
                        await frm.UpdateDataAsync();
                        break;
                    case ChildFormType.ResultEquipments:
                        await frm.UpdateDataAsync();
                        break;
                }
            });
        }

        private async Task EquipmentDeleteAsync(Equipment equipment)
        {
            var name = equipment.Name;

            if (!Msg.Question(string.Format(Resources.QuestionEquipmentDelete, name))) return;

            if (!await Database.Actions.EquipmentDeleteAsync(equipment)) return;

            Utils.Forms.GetChildForms<FrmList>().ForEach(async frm =>
            {
                switch (frm.FormType)
                {
                    case ChildFormType.EquipmentList:
                        frm.ListItemDelete(equipment);

                        break;
                    case ChildFormType.TrackList:
                    case ChildFormType.ResultEquipments:
                        await frm.UpdateDataAsync();

                        break;
                }
            });
        }
    }
}
using P3tr0viCh.Utils;
using System.Data;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameEquipments = "equipments";

        private DataTableFile dtfEquipments = CreateDataTableFile();

        private static DataTableFile CreateDataTableFile()
        {
            var table = new DataTable()
            {
                TableName = "Equipments"
            };

            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Text",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Brand",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Model",
            });

            return new DataTableFile()
            {
                Table = table,
                Author = Utils.AssemblyNameAndVersion(),
            };
        }

        private async Task SaveEquipmentsAsExcelXmlAsync()
        {
            DebugWrite.Line("start");

            var equipments = await Database.Default.ListLoadAsync<Equipment>();

            var fileName = GetSaveFileName(fileNameEquipments, FileType.ExcelXml);

            foreach (var equipment in equipments)
            {
                var row = dtfEquipments.Table.NewRow();

                row["Text"] = equipment.Text;
                row["Brand"] = equipment.Brand;
                row["Model"] = equipment.Model;

                dtfEquipments.Table.Rows.Add(row);
            }

            dtfEquipments.FileName = fileName;

            dtfEquipments.WriteToExcelXml();

            DebugWrite.Line("end");
        }

        private async Task SaveEquipmentsAsync()
        {
            if (Settings.Equipments == default)
            {
                return;
            }

            await SaveEquipmentsAsExcelXmlAsync();
        }

        private void LoadEquipments()
        {
            if (Settings.Equipments == default)
            {
                return;
            }

            DebugWrite.Line("start");

            dtfEquipments.FileName = GetLoadFileName(fileNameEquipments, FileType.ExcelXml);
            
            dtfEquipments.ReadFromExcelXml();

            foreach (DataRow row in dtfEquipments.Table.Rows)
            {
                DebugWrite.Line($"{row["Text"]}: {row["Brand"]} — {row["Model"]}");
            }

            DebugWrite.Line("end");
        }
    }
}
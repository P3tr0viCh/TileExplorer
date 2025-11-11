using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameEquipments = "equipments";

        private DataTableFile dtfEquipments = CreateDataTableFileEquipments();

        private static DataTableFile CreateDataTableFileEquipments()
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

            foreach (var equipment in equipments)
            {
                var row = dtfEquipments.Table.NewRow();

                row["Text"] = equipment.Text;
                row["Brand"] = equipment.Brand;
                row["Model"] = equipment.Model;

                dtfEquipments.Table.Rows.Add(row);
            }

            dtfEquipments.FileName = GetFullFileName(FileName.Equipments, FileType.ExcelXml);

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

            dtfEquipments.FileName = GetFullFileName(FileName.Equipments, FileType.ExcelXml);
            
            dtfEquipments.ReadFromExcelXml();

            var equipments = new List<Equipment>();

            foreach (DataRow row in dtfEquipments.Table.Rows)
            {
                DebugWrite.Line($"{row["Text"]}: {row["Brand"]} — {row["Model"]}");

                equipments.Add(new Equipment()
                {
                    Text = Convert.ToString(row["Text"]),
                    Brand = Convert.ToString(row["Brand"]),
                    Model = Convert.ToString(row["Model"])
                });
            }

            DebugWrite.Line("end");
        }
    }
}
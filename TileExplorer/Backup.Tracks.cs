using P3tr0viCh.Utils;
using System;
using System.Data;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Backup
    {
        private const string fileNameTrackExts = "trackexts";

        private DataTableFile dtfTrackExts = CreateDataTableFileTrackExts();

        private static DataTableFile CreateDataTableFileTrackExts()
        {
            var table = new DataTable()
            {
                TableName = "TrackExts"
            };

            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(DateTime),
                ColumnName = "DateTime",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Text",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(string),
                ColumnName = "Equipment",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(float),
                ColumnName = "EleAscent",
            });
            table.Columns.Add(new DataColumn()
            {
                DataType = typeof(float),
                ColumnName = "EleDescent",
            });

            return CreateDataTableFile(table);
        }

        private async Task SaveTrackExtsAsExcelXmlAsync()
        {
            DebugWrite.Line("start");

            var tracks = await Database.Default.ListLoadAsync<Track>(new { forBackup = true });

            foreach (var track in tracks)
            {
                var row = dtfTrackExts.Table.NewRow();

                row["DateTime"] = track.DateTimeStart;
                row["Text"] = track.Text;
                row["Equipment"] = track.EquipmentText;
                row["EleAscent"] = track.EleAscent;
                row["EleDescent"] = track.EleDescent;

                dtfTrackExts.Table.Rows.Add(row);
            }

            dtfTrackExts.FileName = GetFullFileName(FileName.TrackExts);

            dtfTrackExts.WriteToExcelXml();

            DebugWrite.Line("end");
        }

        private async Task SaveTrackExtsAsync()
        {
            if (!Settings.FileNames.HasFlag(FileName.TrackExts))
            {
                return;
            }

            await SaveTrackExtsAsExcelXmlAsync();
        }
    }
}
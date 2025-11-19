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

            var tracks = await Database.Default.TrackExtsLoadAsync();

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

        private bool TracksEqual(Track track1, Track track2)
        {
            if (track1.EquipmentId != track2.EquipmentId) return false;
            if (track1.EleAscent != track2.EleAscent) return false;
            if (track1.EleDescent != track2.EleDescent) return false;

            return true;
        }

        private async Task LoadTrackExtsAsync()
        {
            if (!Settings.FileNames.HasFlag(FileName.TrackExts))
            {
                return;
            }

            DebugWrite.Line("start");

            dtfTrackExts.FileName = GetFullFileName(FileName.TrackExts);

            dtfTrackExts.ReadFromExcelXml();

            var equipments = await Database.Default.ListLoadAsync<Equipment>();

            var tracks = await Database.Default.ListLoadAsync<Track>();

#if DEBUG            
            tracks.ForEach(track =>
            {
                //DebugWrite.Line($"{track.DateTimeStart}: {track.Text}, {track.EquipmentId}, {track.EleAscent}");
            });
#endif

            var tracksFromBackup = new List<Track>();

            foreach (DataRow row in dtfTrackExts.Table.Rows)
            {
                tracksFromBackup.Add(new Track()
                {
                    DateTimeStart = Convert.ToDateTime(row["DateTime"]),
                    Text = Convert.ToString(row["Text"]),
                    EquipmentText = Convert.ToString(row["Equipment"]),
                    EleAscent = Convert.ToSingle(row["EleAscent"]),
                    EleDescent = Convert.ToSingle(row["EleDescent"]),
                });
            }

            Equipment equipment;

            foreach (var track in tracksFromBackup)
            {
                DebugWrite.Line($"{track.DateTimeStart}: {track.Text}, {track.EquipmentText} ({track.EquipmentId})");

                if (track.EquipmentText.IsEmpty()) continue;

                equipment = equipments.Find(t => t.Text == track.EquipmentText);

                if (equipment is null)
                {
                    equipment = new Equipment()
                    {
                        Text = track.EquipmentText,
                    };

                    await Database.Default.ListItemSaveAsync(equipment);

                    equipments.Add(equipment);
                }

                track.Equipment = equipment;

                DebugWrite.Line($"{track.DateTimeStart}: {track.Text}, {track.EquipmentText} ({track.EquipmentId})");
            }

            var tracksForUpdate = new List<Track>();

            Track trackFromBackup;

            foreach (var track in tracks)
            {
                trackFromBackup = tracksFromBackup.Find(t => t.DateTimeStart == track.DateTimeStart);

                if (trackFromBackup == null) continue;

                if (TracksEqual(track, trackFromBackup)) continue;

                track.Equipment = trackFromBackup.Equipment;
                track.EleAscent = trackFromBackup.EleAscent;
                track.EleDescent = trackFromBackup.EleDescent;

                tracksForUpdate.Add(track);
            }

#if DEBUG
            DebugWrite.Line($"{tracksForUpdate.Count}");

            tracksForUpdate.ForEach(track =>
            {
                DebugWrite.Line($"{track.DateTimeStart}: {track.Text}, {track.EquipmentId}, {track.EleAscent}");
            });
#endif

            await Database.Default.TrackExtsSaveAsync(tracksForUpdate);

            DebugWrite.Line("end");
        }
    }
}
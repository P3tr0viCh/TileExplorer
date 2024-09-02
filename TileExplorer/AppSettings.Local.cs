using P3tr0viCh.Utils;
using static TileExplorer.Backup;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        internal class Local : SettingsBase<Local>
        {
            public readonly Database.Filter Filter = new Database.Filter();

            public string DatabaseHome { get; set; } = string.Empty;

            // ------------------------------------------------------------------------------------------------------------
            public string DirectoryMapImage { get; set; } = string.Empty;
            public string DirectoryTileBoundary { get; set; } = string.Empty;
            public string DirectoryTileStatus { get; set; } = string.Empty;

            // ------------------------------------------------------------------------------------------------------------
            public double HomeLat { get; set; } = 51.196369;
            public double HomeLng { get; set; } = 58.298527;
            public int HomeZoom { get; set; } = 11;

            // ------------------------------------------------------------------------------------------------------------
            public bool MapGrayScale { get; set; } = false;

            // ------------------------------------------------------------------------------------------------------------
            public bool VisibleGrid { get; set; } = true;
            public bool VisibleMarkers { get; set; } = true;
            public bool VisibleTracks { get; set; } = true;
            public bool VisibleTiles { get; set; } = true;
            public bool VisibleFilter { get; set; } = true;
            public bool VisibleMarkerList { get; set; } = true;
            public bool VisibleTrackList { get; set; } = true;
            public bool VisibleEquipmentList { get; set; } = true;
            public bool VisibleResultYears { get; set; } = true;
            public bool VisibleResultEquipments { get; set; } = true;
            public bool VisibleTracksTree { get; set; } = true;

            // ------------------------------------------------------------------------------------------------------------
            public bool VisibleLeftPanel { get; set; } = true;

            // ------------------------------------------------------------------------------------------------------------
            public FormState FormStateMain { get; set; }
            public FormState FormStateResultYears { get; set; }
            public FormState FormStateResultEquipments { get; set; }
            public FormState FormStateTrackList { get; set; }
            public FormState FormStateMarkerList { get; set; }
            public FormState FormStateEquipmentList { get; set; }
            public FormState FormStateFilter { get; set; }
            public FormState FormStateSettings { get; set; }
            public FormState FormStateTracksTree { get; set; }

            // ------------------------------------------------------------------------------------------------------------
            public ColumnState[] ColumnsResultYears { get; set; }
            public ColumnState[] ColumnsResultEquipments { get; set; }
            public ColumnState[] ColumnsTrackList { get; set; }
            public ColumnState[] ColumnsMarkerList { get; set; }
            public ColumnState[] ColumnsEquipmentList { get; set; }

            // ------------------------------------------------------------------------------------------------------------
            private readonly BackupSettings backupSettings = new BackupSettings();

            public BackupSettings BackupSettings
            {
                get => backupSettings;
                set => backupSettings.Assign(value);
            }
        }
    }
}
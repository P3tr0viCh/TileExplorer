using P3tr0viCh.Utils;
using static TileExplorer.Backup;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        internal class Local : SettingsBase<Local>
        {
            public readonly Database.Filter Filter = new Database.Filter();

            public string DirectoryDatabase { get; set; } = string.Empty;

            // ------------------------------------------------------------------------------------------------------------
            public string DirectoryLastTracks { get; set; } = string.Empty;
            public string DirectoryLastMapImage { get; set; } = string.Empty;
            public string DirectoryLastTileBoundary { get; set; } = string.Empty;
            public string DirectoryLastTileStatus { get; set; } = string.Empty;
            public string DirectoryTracks { get; set; } = string.Empty;
            public string DirectoryRoaming { get; set; } = string.Empty;

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
            public FormState FormStateTileInfo { get; set; }

            // ------------------------------------------------------------------------------------------------------------
            public ColumnState[] ColumnsResultYears { get; set; }
            public ColumnState[] ColumnsResultEquipments { get; set; }
            public ColumnState[] ColumnsTrackList { get; set; }
            public ColumnState[] ColumnsMarkerList { get; set; }
            public ColumnState[] ColumnsEquipmentList { get; set; }
            public ColumnState[] ColumnsTileInfo { get; set; }

            // ------------------------------------------------------------------------------------------------------------
            public BackupSettings BackupSettings = new BackupSettings();
        }
    }
}
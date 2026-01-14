using P3tr0viCh.Utils.Settings;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        internal class Local : SettingsBase<Local>
        {
            public readonly Database.Filter Filter = new Database.Filter();

            public string DirectoryDatabase { get; set; } = string.Empty;

            // --------------------------------------------------------------------------
            public string DirectoryLastTracks { get; set; } = string.Empty;
            public string DirectoryLastMapImage { get; set; } = string.Empty;
            public string DirectoryLastTileBoundary { get; set; } = string.Empty;
            public string DirectoryLastTileStatus { get; set; } = string.Empty;
            public string DirectoryTracks { get; set; } = string.Empty;
            public string DirectoryRoaming { get; set; } = string.Empty;
            public string DirectoryBackups { get; set; } = string.Empty;

            // --------------------------------------------------------------------------
            public double HomeLat { get; set; } = 51.196369;
            public double HomeLng { get; set; } = 58.298527;
            public int HomeZoom { get; set; } = 11;

            // --------------------------------------------------------------------------
            public bool MapGrayScale { get; set; } = false;
            public bool MapTilesHeatmap { get; set; } = false;

            // --------------------------------------------------------------------------
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

            // --------------------------------------------------------------------------
            public bool VisibleLeftPanel { get; set; } = true;

            // --------------------------------------------------------------------------
            public FormStates FormStates { get; private set; } = new FormStates();

            public ColumnStates ColumnStates { get; private set; } = new ColumnStates();

            // --------------------------------------------------------------------------
            protected override void Check()
            {
                if (FormStates == null)
                {
                    FormStates = new FormStates();
                }

                if (ColumnStates == null)
                {
                    ColumnStates = new ColumnStates();
                }
            }
        }
    }
}
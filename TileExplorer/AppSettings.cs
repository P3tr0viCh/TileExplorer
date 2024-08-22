using P3tr0viCh.Utils;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using static TileExplorer.Backup;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal class AppSettings : SettingsBase<AppSettings>
    {
        public readonly Database.Filter Filter = new Database.Filter();

        [LocalizedAttribute.Category("Category.Common", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("DatabaseHome.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("DatabaseHome.Description", "Properties.Resources.Settings")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DatabaseHome { get; set; } = string.Empty;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerFill { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerFillAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerFillAlpha { get; set; } = 220;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerText.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerText.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerText { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerTextAlpha { get; set; } = 255;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerTextFill { get; set; } = Color.LightGray;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFillAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerTextFillAlpha { get; set; } = 220;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerLine.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerLine { get; set; } = Color.Gray;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerSelectedFill { get; set; } = Color.DarkSlateBlue;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFillAlphaDisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerSelectedFillAlpha { get; set; } = 220;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerLineAlpha { get; set; } = 160;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedLine.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerSelectedLine { get; set; } = Color.DarkSlateBlue;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerSelectedLineAlpha { get; set; } = 220;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthMarkerLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthMarkerLine.Description", "Properties.Resources.Settings")]
        public int WidthMarkerLine { get; set; } = 1;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthMarkerLineSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthMarkerLineSelected.Description", "Properties.Resources.Settings")]
        public int WidthMarkerLineSelected { get; set; } = 2;

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FontMarker.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FontMarker.Description", "Properties.Resources.Settings")]
        public Font FontMarker { get; set; } = new Font("Arial", 10);

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileCluster.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileCluster.Description", "Properties.Resources.Settings")]
        public Color ColorTileCluster { get; set; } = Color.FromArgb(220, 220, 20);

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileClusterAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileClusterAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileClusterAlpha { get; set; } = 40;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileClusterLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileClusterLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileClusterLineAlpha { get; set; } = 50;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxCluster.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxCluster.Description", "Properties.Resources.Settings")]
        public Color ColorTileMaxCluster { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxClusterAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxClusterAlpha { get; set; } = 40;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxClusterLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxClusterLineAlpha { get; set; } = 50;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquare.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquare.Description", "Properties.Resources.Settings")]
        public Color ColorTileMaxSquare { get; set; } = Color.FromArgb(66, 140, 244);

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquareAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxSquareAlpha { get; set; } = 25;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquareLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxSquareLineAlpha { get; set; } = 50;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisited.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisited.Description", "Properties.Resources.Settings")]
        public Color ColorTileVisited { get; set; } = Color.Red;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisitedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisitedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileVisitedAlpha { get; set; } = 25;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisitedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisitedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileVisitedLineAlpha { get; set; } = 25;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelected.Description", "Properties.Resources.Settings")]
        public Color ColorTileTrackSelected { get; set; } = Color.DarkSlateBlue;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelectedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileTrackSelectedAlpha { get; set; } = 144;

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelectedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileTrackSelectedLineAlpha { get; set; } = 88;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrack.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrack.Description", "Properties.Resources.Settings")]
        public Color ColorTrack { get; set; } = Color.Red;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTrackAlpha { get; set; } = 144;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackSelected.Description", "Properties.Resources.Settings")]
        public Color ColorTrackSelected { get; set; } = Color.DarkSlateBlue;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackSelectedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackSelectedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTrackSelectedAlpha { get; set; } = 220;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthTrack.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthTrack.Description", "Properties.Resources.Settings")]
        public int WidthTrack { get; set; } = 2;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthTrackSelected.Description", "Properties.Resources.Settings")]
        public int WidthTrackSelected { get; set; } = 4;

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TrackMinDistancePoint.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TrackMinDistancePoint.Description", "Properties.Resources.Settings")]
        public int TrackMinDistancePoint { get; set; } = 100;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDate.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDate.Description", "Properties.Resources.Settings")]
        public string FormatDate { get; set; } = "yyyy.MM.dd";

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatTime.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatTime.Description", "Properties.Resources.Settings")]
        public string FormatTime { get; set; } = "HH:mm";

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDateTime.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDateTime.Description", "Properties.Resources.Settings")]
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDistance.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDistance.Description", "Properties.Resources.Settings")]
        public string FormatDistance { get; set; } = "0.00";
        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDistance2.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDistance2.Description", "Properties.Resources.Settings")]
        public string FormatDistance2 { get; set; } = "0";

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatEleAscent.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatEleAscent.Description", "Properties.Resources.Settings")]
        public string FormatEleAscent { get; set; } = "0";

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatLatLng.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatLatLng.Description", "Properties.Resources.Settings")]
        public string FormatLatLng { get; set; } = "0.000000";

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("OsmTileKey.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("OsmTileKey.Description", "Properties.Resources.Settings")]
        public string OsmTileKey { get; set; } = "boundary";

        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("OsmTileValue.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("OsmTileValue.Description", "Properties.Resources.Settings")]
        public string OsmTileValue { get; set; } = "tile";

        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("SaveOsmTileMinZoom.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("SaveOsmTileMinZoom.Description", "Properties.Resources.Settings")]
        public int SaveOsmTileMinZoom { get; set; } = 10;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.TileStatus", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileWptType.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileWptType.Description", "Properties.Resources.Settings")]
        public string TileStatusFileWptType { get; set; } = string.Empty;

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileUseOsmand.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileUseOsmand.Description", "Properties.Resources.Settings")]
        public bool TileStatusFileUseOsmand { get; set; } = true;

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIcon.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIcon.Description", "Properties.Resources.Settings")]
        public string TileStatusFileOsmandIcon { get; set; } = "special_star";

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconBackground.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconBackground.Description", "Properties.Resources.Settings")]
        public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get; set; } = OsmandIconBackgroud.Circle;

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconColor.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconColor.Description", "Properties.Resources.Settings")]
        public Color TileStatusFileOsmandIconColor { get; set; } = Color.Red;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public double HomeLat { get; set; } = 51.196369;
        [Browsable(false)]
        public double HomeLng { get; set; } = 58.298527;
        [Browsable(false)]
        public int HomeZoom { get; set; } = 11;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public bool MapGrayScale { get; set; } = false;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public byte MouseWheelZoomType { get; set; } = 1;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public string MapRefererUrl { get; set; } = "https://github.com/P3tr0viCh/TileExplorer";

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public bool VisibleGrid { get; set; } = true;
        [Browsable(false)]
        public bool VisibleMarkers { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTracks { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTiles { get; set; } = true;
        [Browsable(false)]
        public bool VisibleFilter { get; set; } = true;
        [Browsable(false)]
        public bool VisibleMarkerList { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTrackList { get; set; } = true;
        [Browsable(false)]
        public bool VisibleEquipmentList { get; set; } = true;
        [Browsable(false)]
        public bool VisibleResultYears { get; set; } = true;
        [Browsable(false)]
        public bool VisibleResultEquipments { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTracksTree { get; set; } = true;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public bool VisibleLeftPanel { get; set; } = true;

        // ------------------------------------------------------------------------------------------------------------
        private readonly BackupSettings backupSettings = new BackupSettings();
       
        [Browsable(false)]
        public BackupSettings BackupSettings
        {
            get
            {
                return backupSettings;
            }
            set
            {
                backupSettings.Assign(value);
            }
        }

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public FormState FormStateMain { get; set; }
        [Browsable(false)]
        public FormState FormStateResultYears { get; set; }
        [Browsable(false)]
        public FormState FormStateResultEquipments { get; set; }
        [Browsable(false)]
        public FormState FormStateTrackList { get; set; }
        [Browsable(false)]
        public FormState FormStateMarkerList { get; set; }
        [Browsable(false)]
        public FormState FormStateEquipmentList { get; set; }
        [Browsable(false)]
        public FormState FormStateFilter { get; set; }
        [Browsable(false)]
        public FormState FormStateSettings { get; set; }
        [Browsable(false)]
        public FormState FormStateTracksTree { get; set; }

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public ColumnState[] ColumnsResultYears { get; set; }
        [Browsable(false)]
        public ColumnState[] ColumnsResultEquipments { get; set; }
        [Browsable(false)]
        public ColumnState[] ColumnsTrackList { get; set; }
        [Browsable(false)]
        public ColumnState[] ColumnsMarkerList { get; set; }
        [Browsable(false)]
        public ColumnState[] ColumnsEquipmentList { get; set; }
    }
}
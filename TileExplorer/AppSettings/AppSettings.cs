using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Attributes;
using P3tr0viCh.Utils.Settings;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;

namespace TileExplorer
{
    internal class AppSettings : DefaultInstance<AppSettings>, ISettingsStore
    {
        private const string Resource = "Properties.ResourcesSettings";

        private static readonly SettingsStoreLocal storeLocal = new SettingsStoreLocal();
        private static readonly SettingsStoreRoaming storeRoaming = new SettingsStoreRoaming();

        public static SettingsStoreLocal StoreLocal => storeLocal;
        public static SettingsStoreRoaming StoreRoaming => storeRoaming;

        public static SettingsLocal Local => StoreLocal.Settings;
        public static SettingsRoaming Roaming => StoreRoaming.Settings;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryDatabase.DisplayName", Resource)]
        [LocalizedDescription("DirectoryDatabase.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryDatabase { get => Local.DirectoryDatabase; set => Local.DirectoryDatabase = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryTracks.DisplayName", Resource)]
        [LocalizedDescription("DirectoryTracks.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryTracks { get => Local.DirectoryTracks; set => Local.DirectoryTracks = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryRoaming.DisplayName", Resource)]
        [LocalizedDescription("DirectoryRoaming.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryRoaming { get => Local.DirectoryRoaming; set => Local.DirectoryRoaming = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryBackups.DisplayName", Resource)]
        [LocalizedDescription("DirectoryBackups.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory(CheckExists = false)]
        public string DirectoryBackups { get => Local.DirectoryBackups; set => Local.DirectoryBackups = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerFill.Description", Resource)]
        public Color ColorMarkerFill { get => Roaming.ColorMarkerFill; set => Roaming.ColorMarkerFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerFillAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerFillAlpha.Description", Resource)]
        public byte ColorMarkerFillAlpha { get => Roaming.ColorMarkerFillAlpha; set => Roaming.ColorMarkerFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerText.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerText.Description", Resource)]
        public Color ColorMarkerText { get => Roaming.ColorMarkerText; set => Roaming.ColorMarkerText = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextAlpha.Description", Resource)]
        public byte ColorMarkerTextAlpha { get => Roaming.ColorMarkerTextAlpha; set => Roaming.ColorMarkerTextAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextFill.Description", Resource)]
        public Color ColorMarkerTextFill { get => Roaming.ColorMarkerTextFill; set => Roaming.ColorMarkerTextFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextFillAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextFillAlpha.Description", Resource)]
        public byte ColorMarkerTextFillAlpha { get => Roaming.ColorMarkerTextFillAlpha; set => Roaming.ColorMarkerTextFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerLine.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerLine.Description", Resource)]
        public Color ColorMarkerLine { get => Roaming.ColorMarkerLine; set => Roaming.ColorMarkerLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedFill.Description", Resource)]
        public Color ColorMarkerSelectedFill { get => Roaming.ColorMarkerSelectedFill; set => Roaming.ColorMarkerSelectedFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedFillAlphaDisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedFillAlpha.Description", Resource)]
        public byte ColorMarkerSelectedFillAlpha { get => Roaming.ColorMarkerSelectedFillAlpha; set => Roaming.ColorMarkerSelectedFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerLineAlpha.Description", Resource)]
        public byte ColorMarkerLineAlpha { get => Roaming.ColorMarkerLineAlpha; set => Roaming.ColorMarkerLineAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedLine.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedLine.Description", Resource)]
        public Color ColorMarkerSelectedLine { get => Roaming.ColorMarkerSelectedLine; set => Roaming.ColorMarkerSelectedLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedLineAlpha.Description", Resource)]
        public byte ColorMarkerSelectedLineAlpha { get => Roaming.ColorMarkerSelectedLineAlpha; set => Roaming.ColorMarkerSelectedLineAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerPosition.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerPosition.Description", Resource)]
        public Color ColorMarkerPosition { get => Roaming.ColorMarkerPosition; set => Roaming.ColorMarkerPosition = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("WidthMarkerLine.DisplayName", Resource)]
        [LocalizedDescription("WidthMarkerLine.Description", Resource)]
        public int WidthMarkerLine { get => Roaming.WidthMarkerLine; set => Roaming.WidthMarkerLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("WidthMarkerLineSelected.DisplayName", Resource)]
        [LocalizedDescription("WidthMarkerLineSelected.Description", Resource)]
        public int WidthMarkerLineSelected { get => Roaming.WidthMarkerLineSelected; set => Roaming.WidthMarkerLineSelected = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("FontMarker.DisplayName", Resource)]
        [LocalizedDescription("FontMarker.Description", Resource)]
        public Font FontMarker { get => Roaming.FontMarker; set => Roaming.FontMarker = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileCluster.DisplayName", Resource)]
        [LocalizedDescription("ColorTileCluster.Description", Resource)]
        public Color ColorTileCluster { get => Roaming.ColorTileCluster; set => Roaming.ColorTileCluster = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileClusterAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileClusterAlpha.Description", Resource)]
        public byte ColorTileClusterAlpha { get => Roaming.ColorTileClusterAlpha; set => Roaming.ColorTileClusterAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileClusterLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileClusterLineAlpha.Description", Resource)]
        public byte ColorTileClusterLineAlpha { get => Roaming.ColorTileClusterLineAlpha; set => Roaming.ColorTileClusterLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxCluster.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxCluster.Description", Resource)]
        public Color ColorTileMaxCluster { get => Roaming.ColorTileMaxCluster; set => Roaming.ColorTileMaxCluster = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxClusterAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxClusterAlpha.Description", Resource)]
        public byte ColorTileMaxClusterAlpha { get => Roaming.ColorTileMaxClusterAlpha; set => Roaming.ColorTileMaxClusterAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxClusterLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxClusterLineAlpha.Description", Resource)]
        public byte ColorTileMaxClusterLineAlpha { get => Roaming.ColorTileMaxClusterLineAlpha; set => Roaming.ColorTileMaxClusterLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquare.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquare.Description", Resource)]
        public Color ColorTileMaxSquare { get => Roaming.ColorTileMaxSquare; set => Roaming.ColorTileMaxSquare = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquareAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquareAlpha.Description", Resource)]
        public byte ColorTileMaxSquareAlpha { get => Roaming.ColorTileMaxSquareAlpha; set => Roaming.ColorTileMaxSquareAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquareLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquareLineAlpha.Description", Resource)]
        public byte ColorTileMaxSquareLineAlpha { get => Roaming.ColorTileMaxSquareLineAlpha; set => Roaming.ColorTileMaxSquareLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisited.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisited.Description", Resource)]
        public Color ColorTileVisited { get => Roaming.ColorTileVisited; set => Roaming.ColorTileVisited = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisitedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisitedAlpha.Description", Resource)]
        public byte ColorTileVisitedAlpha { get => Roaming.ColorTileVisitedAlpha; set => Roaming.ColorTileVisitedAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisitedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisitedLineAlpha.Description", Resource)]
        public byte ColorTileVisitedLineAlpha { get => Roaming.ColorTileVisitedLineAlpha; set => Roaming.ColorTileVisitedLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelected.Description", Resource)]
        public Color ColorTileTrackSelected { get => Roaming.ColorTileTrackSelected; set => Roaming.ColorTileTrackSelected = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelectedAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedAlpha { get => Roaming.ColorTileTrackSelectedAlpha; set => Roaming.ColorTileTrackSelectedAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelectedLineAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedLineAlpha { get => Roaming.ColorTileTrackSelectedLineAlpha; set => Roaming.ColorTileTrackSelectedLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmap.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmap.Description", Resource)]
        public Color ColorTileHeatmap { get => Roaming.ColorTileHeatmap; set => Roaming.ColorTileHeatmap = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapLineAlpha.Description", Resource)]
        public byte ColorTileHeatmapLineAlpha { get => Roaming.ColorTileHeatmapLineAlpha; set => Roaming.ColorTileHeatmapLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapMinAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapMinAlpha.Description", Resource)]
        public byte ColorTileHeatmapMinAlpha { get => Roaming.ColorTileHeatmapMinAlpha; set => Roaming.ColorTileHeatmapMinAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapMaxAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapMaxAlpha.Description", Resource)]
        public byte ColorTileHeatmapMaxAlpha { get => Roaming.ColorTileHeatmapMaxAlpha; set => Roaming.ColorTileHeatmapMaxAlpha = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrack.DisplayName", Resource)]
        [LocalizedDescription("ColorTrack.Description", Resource)]
        public Color ColorTrack { get => Roaming.ColorTrack; set => Roaming.ColorTrack = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackAlpha.Description", Resource)]
        public byte ColorTrackAlpha { get => Roaming.ColorTrackAlpha; set => Roaming.ColorTrackAlpha = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackSelected.Description", Resource)]
        public Color ColorTrackSelected { get => Roaming.ColorTrackSelected; set => Roaming.ColorTrackSelected = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackSelectedAlpha.Description", Resource)]
        public byte ColorTrackSelectedAlpha { get => Roaming.ColorTrackSelectedAlpha; set => Roaming.ColorTrackSelectedAlpha = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("WidthTrack.DisplayName", Resource)]
        [LocalizedDescription("WidthTrack.Description", Resource)]
        public int WidthTrack { get => Roaming.WidthTrack; set => Roaming.WidthTrack = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("WidthTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("WidthTrackSelected.Description", Resource)]
        public int WidthTrackSelected { get => Roaming.WidthTrackSelected; set => Roaming.WidthTrackSelected = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDate.DisplayName", Resource)]
        [LocalizedDescription("FormatDate.Description", Resource)]
        public string FormatDate { get => Roaming.FormatDate; set => Roaming.FormatDate = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatTime.DisplayName", Resource)]
        [LocalizedDescription("FormatTime.Description", Resource)]
        public string FormatTime { get => Roaming.FormatTime; set => Roaming.FormatTime = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDateTime.DisplayName", Resource)]
        [LocalizedDescription("FormatDateTime.Description", Resource)]
        public string FormatDateTime { get => Roaming.FormatDateTime; set => Roaming.FormatDateTime = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDistance.DisplayName", Resource)]
        [LocalizedDescription("FormatDistance.Description", Resource)]
        public string FormatDistance { get => Roaming.FormatDistance; set => Roaming.FormatDistance = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDistanceRound.DisplayName", Resource)]
        [LocalizedDescription("FormatDistanceRound.Description", Resource)]
        public string FormatDistanceRound { get => Roaming.FormatDistanceRound; set => Roaming.FormatDistanceRound = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatEleAscent.DisplayName", Resource)]
        [LocalizedDescription("FormatEleAscent.Description", Resource)]
        public string FormatEleAscent { get => Roaming.FormatEleAscent; set => Roaming.FormatEleAscent = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatEleAscentRound.DisplayName", Resource)]
        [LocalizedDescription("FormatEleAscentRound.Description", Resource)]
        public string FormatEleAscentRound { get => Roaming.FormatEleAscentRound; set => Roaming.FormatEleAscentRound = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatLatLng.DisplayName", Resource)]
        [LocalizedDescription("FormatLatLng.Description", Resource)]
        public string FormatLatLng { get => Roaming.FormatLatLng; set => Roaming.FormatLatLng = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatSpeed.DisplayName", Resource)]
        [LocalizedDescription("FormatSpeed.Description", Resource)]
        public string FormatSpeed { get => Roaming.FormatSpeed; set => Roaming.FormatSpeed = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("OsmTileKey.DisplayName", Resource)]
        [LocalizedDescription("OsmTileKey.Description", Resource)]
        public string OsmTileKey { get => Roaming.OsmTileKey; set => Roaming.OsmTileKey = value; }

        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("OsmTileValue.DisplayName", Resource)]
        [LocalizedDescription("OsmTileValue.Description", Resource)]
        public string OsmTileValue { get => Roaming.OsmTileValue; set => Roaming.OsmTileValue = value; }

        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("SaveOsmTileMinZoom.DisplayName", Resource)]
        [LocalizedDescription("SaveOsmTileMinZoom.Description", Resource)]
        public int SaveOsmTileMinZoom { get => Roaming.SaveOsmTileMinZoom; set => Roaming.SaveOsmTileMinZoom = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.TileStatus", Resource)]
        [LocalizedDisplayName("TileStatusFileWptType.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileWptType.Description", Resource)]
        public string TileStatusFileWptType { get => Roaming.TileStatusFileWptType; set => Roaming.TileStatusFileWptType = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileUseOsmand.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileUseOsmand.Description", Resource)]
        public bool TileStatusFileUseOsmand { get => Roaming.TileStatusFileUseOsmand; set => Roaming.TileStatusFileUseOsmand = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIcon.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIcon.Description", Resource)]
        public string TileStatusFileOsmandIcon { get => Roaming.TileStatusFileOsmandIcon; set => Roaming.TileStatusFileOsmandIcon = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIconBackground.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIconBackground.Description", Resource)]
        public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get => Roaming.TileStatusFileOsmandIconBackground; set => Roaming.TileStatusFileOsmandIconBackground = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIconColor.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIconColor.Description", Resource)]
        public Color TileStatusFileOsmandIconColor { get => Roaming.TileStatusFileOsmandIconColor; set => Roaming.TileStatusFileOsmandIconColor = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartSerial.DisplayName", Resource)]
        public Color ColorChartSerial { get => Roaming.ColorChartSerial; set => Roaming.ColorChartSerial = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartSerialAlpha.DisplayName", Resource)]
        public byte ColorChartSerialAlpha { get => Roaming.ColorChartSerialAlpha; set => Roaming.ColorChartSerialAlpha = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartTracksByYearSerial.DisplayName", Resource)]
        public Color ColorChartTracksByYearSerial { get => Roaming.ColorChartTracksByYearSerial; set => Roaming.ColorChartTracksByYearSerial = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartTracksByYearSerialAlpha.DisplayName", Resource)]
        public byte ColorChartTracksByYearSerialAlpha { get => Roaming.ColorChartTracksByYearSerialAlpha; set => Roaming.ColorChartTracksByYearSerialAlpha = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartCursor.DisplayName", Resource)]
        public Color ColorChartCursor { get => Roaming.ColorChartCursor; set => Roaming.ColorChartCursor = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartAxis.DisplayName", Resource)]
        public Color ColorChartAxis { get => Roaming.ColorChartAxis; set => Roaming.ColorChartAxis = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartGrid.DisplayName", Resource)]
        public Color ColorChartGrid { get => Roaming.ColorChartGrid; set => Roaming.ColorChartGrid = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartText.DisplayName", Resource)]
        [LocalizedDescription("ColorChartText.Description", Resource)]
        public Color ColorChartText { get => Roaming.ColorChartText; set => Roaming.ColorChartText = value; }

        // ------------------------------------------------------------------------------------------------------------
        public bool Save() => StoreLocal.Save() && StoreRoaming.Save();

        public bool Load() => StoreLocal.Load() && StoreRoaming.Load();

        private string GetDirectory(string directory, string defDirectory)
        {
            if (Directory.Exists(directory)) return directory;

            return defDirectory;
        }

        public void UpdateDirectoryRoaming()
        {
            StoreRoaming.Directory = GetDirectory(Local.DirectoryRoaming,
#if DEBUG
                Path.Combine(Files.ExecutableDirectory(), "roaming"));
#else
                Files.AppDataRoamingDirectory());
#endif
        }
    }
}
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Attributes;
using P3tr0viCh.Utils.Settings;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal partial class AppSettings : ISettingsBase
    {
        private const string Resource = "Properties.ResourcesSettings";

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryDatabase.DisplayName", Resource)]
        [LocalizedDescription("DirectoryDatabase.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryDatabase { get => Local.Default.DirectoryDatabase; set => Local.Default.DirectoryDatabase = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryTracks.DisplayName", Resource)]
        [LocalizedDescription("DirectoryTracks.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryTracks { get => Local.Default.DirectoryTracks; set => Local.Default.DirectoryTracks = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryRoaming.DisplayName", Resource)]
        [LocalizedDescription("DirectoryRoaming.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryRoaming { get => Local.Default.DirectoryRoaming; set => Local.Default.DirectoryRoaming = value; }

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryBackups.DisplayName", Resource)]
        [LocalizedDescription("DirectoryBackups.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory(CheckExists = false)]
        public string DirectoryBackups { get => Local.Default.DirectoryBackups; set => Local.Default.DirectoryBackups = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerFill.Description", Resource)]
        public Color ColorMarkerFill { get => Roaming.Default.ColorMarkerFill; set => Roaming.Default.ColorMarkerFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerFillAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerFillAlpha.Description", Resource)]
        public byte ColorMarkerFillAlpha { get => Roaming.Default.ColorMarkerFillAlpha; set => Roaming.Default.ColorMarkerFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerText.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerText.Description", Resource)]
        public Color ColorMarkerText { get => Roaming.Default.ColorMarkerText; set => Roaming.Default.ColorMarkerText = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextAlpha.Description", Resource)]
        public byte ColorMarkerTextAlpha { get => Roaming.Default.ColorMarkerTextAlpha; set => Roaming.Default.ColorMarkerTextAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextFill.Description", Resource)]
        public Color ColorMarkerTextFill { get => Roaming.Default.ColorMarkerTextFill; set => Roaming.Default.ColorMarkerTextFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerTextFillAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerTextFillAlpha.Description", Resource)]
        public byte ColorMarkerTextFillAlpha { get => Roaming.Default.ColorMarkerTextFillAlpha; set => Roaming.Default.ColorMarkerTextFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerLine.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerLine.Description", Resource)]
        public Color ColorMarkerLine { get => Roaming.Default.ColorMarkerLine; set => Roaming.Default.ColorMarkerLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedFill.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedFill.Description", Resource)]
        public Color ColorMarkerSelectedFill { get => Roaming.Default.ColorMarkerSelectedFill; set => Roaming.Default.ColorMarkerSelectedFill = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedFillAlphaDisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedFillAlpha.Description", Resource)]
        public byte ColorMarkerSelectedFillAlpha { get => Roaming.Default.ColorMarkerSelectedFillAlpha; set => Roaming.Default.ColorMarkerSelectedFillAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerLineAlpha.Description", Resource)]
        public byte ColorMarkerLineAlpha { get => Roaming.Default.ColorMarkerLineAlpha; set => Roaming.Default.ColorMarkerLineAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedLine.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedLine.Description", Resource)]
        public Color ColorMarkerSelectedLine { get => Roaming.Default.ColorMarkerSelectedLine; set => Roaming.Default.ColorMarkerSelectedLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerSelectedLineAlpha.Description", Resource)]
        public byte ColorMarkerSelectedLineAlpha { get => Roaming.Default.ColorMarkerSelectedLineAlpha; set => Roaming.Default.ColorMarkerSelectedLineAlpha = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("ColorMarkerPosition.DisplayName", Resource)]
        [LocalizedDescription("ColorMarkerPosition.Description", Resource)]
        public Color ColorMarkerPosition { get => Roaming.Default.ColorMarkerPosition; set => Roaming.Default.ColorMarkerPosition = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("WidthMarkerLine.DisplayName", Resource)]
        [LocalizedDescription("WidthMarkerLine.Description", Resource)]
        public int WidthMarkerLine { get => Roaming.Default.WidthMarkerLine; set => Roaming.Default.WidthMarkerLine = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("WidthMarkerLineSelected.DisplayName", Resource)]
        [LocalizedDescription("WidthMarkerLineSelected.Description", Resource)]
        public int WidthMarkerLineSelected { get => Roaming.Default.WidthMarkerLineSelected; set => Roaming.Default.WidthMarkerLineSelected = value; }

        [LocalizedCategory("Category.DesignMarkers", Resource)]
        [LocalizedDisplayName("FontMarker.DisplayName", Resource)]
        [LocalizedDescription("FontMarker.Description", Resource)]
        public Font FontMarker { get => Roaming.Default.FontMarker; set => Roaming.Default.FontMarker = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileCluster.DisplayName", Resource)]
        [LocalizedDescription("ColorTileCluster.Description", Resource)]
        public Color ColorTileCluster { get => Roaming.Default.ColorTileCluster; set => Roaming.Default.ColorTileCluster = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileClusterAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileClusterAlpha.Description", Resource)]
        public byte ColorTileClusterAlpha { get => Roaming.Default.ColorTileClusterAlpha; set => Roaming.Default.ColorTileClusterAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileClusterLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileClusterLineAlpha.Description", Resource)]
        public byte ColorTileClusterLineAlpha { get => Roaming.Default.ColorTileClusterLineAlpha; set => Roaming.Default.ColorTileClusterLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxCluster.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxCluster.Description", Resource)]
        public Color ColorTileMaxCluster { get => Roaming.Default.ColorTileMaxCluster; set => Roaming.Default.ColorTileMaxCluster = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxClusterAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxClusterAlpha.Description", Resource)]
        public byte ColorTileMaxClusterAlpha { get => Roaming.Default.ColorTileMaxClusterAlpha; set => Roaming.Default.ColorTileMaxClusterAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxClusterLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxClusterLineAlpha.Description", Resource)]
        public byte ColorTileMaxClusterLineAlpha { get => Roaming.Default.ColorTileMaxClusterLineAlpha; set => Roaming.Default.ColorTileMaxClusterLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquare.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquare.Description", Resource)]
        public Color ColorTileMaxSquare { get => Roaming.Default.ColorTileMaxSquare; set => Roaming.Default.ColorTileMaxSquare = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquareAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquareAlpha.Description", Resource)]
        public byte ColorTileMaxSquareAlpha { get => Roaming.Default.ColorTileMaxSquareAlpha; set => Roaming.Default.ColorTileMaxSquareAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileMaxSquareLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileMaxSquareLineAlpha.Description", Resource)]
        public byte ColorTileMaxSquareLineAlpha { get => Roaming.Default.ColorTileMaxSquareLineAlpha; set => Roaming.Default.ColorTileMaxSquareLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisited.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisited.Description", Resource)]
        public Color ColorTileVisited { get => Roaming.Default.ColorTileVisited; set => Roaming.Default.ColorTileVisited = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisitedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisitedAlpha.Description", Resource)]
        public byte ColorTileVisitedAlpha { get => Roaming.Default.ColorTileVisitedAlpha; set => Roaming.Default.ColorTileVisitedAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileVisitedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileVisitedLineAlpha.Description", Resource)]
        public byte ColorTileVisitedLineAlpha { get => Roaming.Default.ColorTileVisitedLineAlpha; set => Roaming.Default.ColorTileVisitedLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelected.Description", Resource)]
        public Color ColorTileTrackSelected { get => Roaming.Default.ColorTileTrackSelected; set => Roaming.Default.ColorTileTrackSelected = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelectedAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedAlpha { get => Roaming.Default.ColorTileTrackSelectedAlpha; set => Roaming.Default.ColorTileTrackSelectedAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileTrackSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileTrackSelectedLineAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedLineAlpha { get => Roaming.Default.ColorTileTrackSelectedLineAlpha; set => Roaming.Default.ColorTileTrackSelectedLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmap.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmap.Description", Resource)]
        public Color ColorTileHeatmap { get => Roaming.Default.ColorTileHeatmap; set => Roaming.Default.ColorTileHeatmap = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapLineAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapLineAlpha.Description", Resource)]
        public byte ColorTileHeatmapLineAlpha { get => Roaming.Default.ColorTileHeatmapLineAlpha; set => Roaming.Default.ColorTileHeatmapLineAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapMinAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapMinAlpha.Description", Resource)]
        public byte ColorTileHeatmapMinAlpha { get => Roaming.Default.ColorTileHeatmapMinAlpha; set => Roaming.Default.ColorTileHeatmapMinAlpha = value; }

        [LocalizedCategory("Category.DesignTiles", Resource)]
        [LocalizedDisplayName("ColorTileHeatmapMaxAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTileHeatmapMaxAlpha.Description", Resource)]
        public byte ColorTileHeatmapMaxAlpha { get => Roaming.Default.ColorTileHeatmapMaxAlpha; set => Roaming.Default.ColorTileHeatmapMaxAlpha = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrack.DisplayName", Resource)]
        [LocalizedDescription("ColorTrack.Description", Resource)]
        public Color ColorTrack { get => Roaming.Default.ColorTrack; set => Roaming.Default.ColorTrack = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackAlpha.Description", Resource)]
        public byte ColorTrackAlpha { get => Roaming.Default.ColorTrackAlpha; set => Roaming.Default.ColorTrackAlpha = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackSelected.Description", Resource)]
        public Color ColorTrackSelected { get => Roaming.Default.ColorTrackSelected; set => Roaming.Default.ColorTrackSelected = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("ColorTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedDescription("ColorTrackSelectedAlpha.Description", Resource)]
        public byte ColorTrackSelectedAlpha { get => Roaming.Default.ColorTrackSelectedAlpha; set => Roaming.Default.ColorTrackSelectedAlpha = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("WidthTrack.DisplayName", Resource)]
        [LocalizedDescription("WidthTrack.Description", Resource)]
        public int WidthTrack { get => Roaming.Default.WidthTrack; set => Roaming.Default.WidthTrack = value; }

        [LocalizedCategory("Category.DesignTracks", Resource)]
        [LocalizedDisplayName("WidthTrackSelected.DisplayName", Resource)]
        [LocalizedDescription("WidthTrackSelected.Description", Resource)]
        public int WidthTrackSelected { get => Roaming.Default.WidthTrackSelected; set => Roaming.Default.WidthTrackSelected = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDate.DisplayName", Resource)]
        [LocalizedDescription("FormatDate.Description", Resource)]
        public string FormatDate { get => Roaming.Default.FormatDate; set => Roaming.Default.FormatDate = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatTime.DisplayName", Resource)]
        [LocalizedDescription("FormatTime.Description", Resource)]
        public string FormatTime { get => Roaming.Default.FormatTime; set => Roaming.Default.FormatTime = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDateTime.DisplayName", Resource)]
        [LocalizedDescription("FormatDateTime.Description", Resource)]
        public string FormatDateTime { get => Roaming.Default.FormatDateTime; set => Roaming.Default.FormatDateTime = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDistance.DisplayName", Resource)]
        [LocalizedDescription("FormatDistance.Description", Resource)]
        public string FormatDistance { get => Roaming.Default.FormatDistance; set => Roaming.Default.FormatDistance = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatDistanceRound.DisplayName", Resource)]
        [LocalizedDescription("FormatDistanceRound.Description", Resource)]
        public string FormatDistanceRound { get => Roaming.Default.FormatDistanceRound; set => Roaming.Default.FormatDistanceRound = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatEleAscent.DisplayName", Resource)]
        [LocalizedDescription("FormatEleAscent.Description", Resource)]
        public string FormatEleAscent { get => Roaming.Default.FormatEleAscent; set => Roaming.Default.FormatEleAscent = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatEleAscentRound.DisplayName", Resource)]
        [LocalizedDescription("FormatEleAscentRound.Description", Resource)]
        public string FormatEleAscentRound { get => Roaming.Default.FormatEleAscentRound; set => Roaming.Default.FormatEleAscentRound = value; }

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("FormatLatLng.DisplayName", Resource)]
        [LocalizedDescription("FormatLatLng.Description", Resource)]
        public string FormatLatLng { get => Roaming.Default.FormatLatLng; set => Roaming.Default.FormatLatLng = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("OsmTileKey.DisplayName", Resource)]
        [LocalizedDescription("OsmTileKey.Description", Resource)]
        public string OsmTileKey { get => Roaming.Default.OsmTileKey; set => Roaming.Default.OsmTileKey = value; }

        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("OsmTileValue.DisplayName", Resource)]
        [LocalizedDescription("OsmTileValue.Description", Resource)]
        public string OsmTileValue { get => Roaming.Default.OsmTileValue; set => Roaming.Default.OsmTileValue = value; }

        [LocalizedCategory("Category.Osm", Resource)]
        [LocalizedDisplayName("SaveOsmTileMinZoom.DisplayName", Resource)]
        [LocalizedDescription("SaveOsmTileMinZoom.Description", Resource)]
        public int SaveOsmTileMinZoom { get => Roaming.Default.SaveOsmTileMinZoom; set => Roaming.Default.SaveOsmTileMinZoom = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.TileStatus", Resource)]
        [LocalizedDisplayName("TileStatusFileWptType.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileWptType.Description", Resource)]
        public string TileStatusFileWptType { get => Roaming.Default.TileStatusFileWptType; set => Roaming.Default.TileStatusFileWptType = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileUseOsmand.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileUseOsmand.Description", Resource)]
        public bool TileStatusFileUseOsmand { get => Roaming.Default.TileStatusFileUseOsmand; set => Roaming.Default.TileStatusFileUseOsmand = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIcon.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIcon.Description", Resource)]
        public string TileStatusFileOsmandIcon { get => Roaming.Default.TileStatusFileOsmandIcon; set => Roaming.Default.TileStatusFileOsmandIcon = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIconBackground.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIconBackground.Description", Resource)]
        public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get => Roaming.Default.TileStatusFileOsmandIconBackground; set => Roaming.Default.TileStatusFileOsmandIconBackground = value; }

        [LocalizedCategory("Category.TileStatusOsmand", Resource)]
        [LocalizedDisplayName("TileStatusFileOsmandIconColor.DisplayName", Resource)]
        [LocalizedDescription("TileStatusFileOsmandIconColor.Description", Resource)]
        public Color TileStatusFileOsmandIconColor { get => Roaming.Default.TileStatusFileOsmandIconColor; set => Roaming.Default.TileStatusFileOsmandIconColor = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartSerial.DisplayName", Resource)]
        public Color ColorChartSerial { get => Roaming.Default.ColorChartSerial; set => Roaming.Default.ColorChartSerial = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartSerialAlpha.DisplayName", Resource)]
        public byte ColorChartSerialAlpha { get => Roaming.Default.ColorChartSerialAlpha; set => Roaming.Default.ColorChartSerialAlpha = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartTracksByYearSerial.DisplayName", Resource)]
        public Color ColorChartTracksByYearSerial { get => Roaming.Default.ColorChartTracksByYearSerial; set => Roaming.Default.ColorChartTracksByYearSerial = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartTracksByYearSerialAlpha.DisplayName", Resource)]
        public byte ColorChartTracksByYearSerialAlpha { get => Roaming.Default.ColorChartTracksByYearSerialAlpha; set => Roaming.Default.ColorChartTracksByYearSerialAlpha = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartCursor.DisplayName", Resource)]
        public Color ColorChartCursor { get => Roaming.Default.ColorChartCursor; set => Roaming.Default.ColorChartCursor = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartAxis.DisplayName", Resource)]
        public Color ColorChartAxis { get => Roaming.Default.ColorChartAxis; set => Roaming.Default.ColorChartAxis = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartGrid.DisplayName", Resource)]
        public Color ColorChartGrid { get => Roaming.Default.ColorChartGrid; set => Roaming.Default.ColorChartGrid = value; }

        [LocalizedCategory("Category.Chart", Resource)]
        [LocalizedDisplayName("ColorChartText.DisplayName", Resource)]
        [LocalizedDescription("ColorChartText.Description", Resource)]
        public Color ColorChartText { get => Roaming.Default.ColorChartText; set => Roaming.Default.ColorChartText = value; }

        // ------------------------------------------------------------------------------------------------------------
        public static bool LocalSave()
        {
            if (Local.Default.Save()) return true;

            DebugWrite.Error(Local.LastError);

            return false;
        }

        public static bool RoamingSave()
        {
            if (Roaming.Default.Save()) return true;

            DebugWrite.Error(Roaming.LastError);

            return false;
        }

        public bool Save() => LocalSave() && RoamingSave();

        private static string GetDirectory(string directory, string defDirectory)
        {
            if (Directory.Exists(directory)) return directory;

            return defDirectory;
        }

        public static void UpdateDirectoryRoaming()
        {
            Roaming.Directory = GetDirectory(Local.Default.DirectoryRoaming,
#if DEBUG
                Path.Combine(Files.ExecutableDirectory(), "roaming"));

            Utils.DirectoryCreate(Roaming.Directory);
#else
                Files.AppDataRoamingDirectory());
#endif
        }

        public static bool LocalLoad()
        {
            DebugWrite.Line($"Settings Local: {Local.FilePath}");

            if (!Local.Default.Load())
            {
                DebugWrite.Error(Local.LastError);

                return false;
            }

            UpdateDirectoryRoaming();

            return true;
        }

        public static bool RoamingLoad()
        {
            DebugWrite.Line($"Settings Roaming: {Roaming.FilePath}");

            if (Roaming.Default.Load()) return true;

            DebugWrite.Error(Roaming.LastError);

            return false;
        }

        public bool Load() => LocalLoad() && RoamingLoad();
    }
}
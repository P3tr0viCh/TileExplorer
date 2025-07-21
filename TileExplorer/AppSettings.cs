using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        private const string Resource = "Properties.ResourcesSettings";

        [LocalizedAttribute.Category("Category.Directories", Resource)]
        [LocalizedAttribute.DisplayName("DirectoryDatabase.DisplayName", Resource)]
        [LocalizedAttribute.Description("DirectoryDatabase.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DirectoryDatabase { get => Local.Default.DirectoryDatabase; set => Local.Default.DirectoryDatabase = value; }

        [LocalizedAttribute.Category("Category.Directories", Resource)]
        [LocalizedAttribute.DisplayName("DirectoryTracks.DisplayName", Resource)]
        [LocalizedAttribute.Description("DirectoryTracks.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DirectoryTracks { get => Local.Default.DirectoryTracks; set => Local.Default.DirectoryTracks = value; }

        [LocalizedAttribute.Category("Category.Directories", Resource)]
        [LocalizedAttribute.DisplayName("DirectoryRoaming.DisplayName", Resource)]
        [LocalizedAttribute.Description("DirectoryRoaming.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DirectoryRoaming { get => Local.Default.DirectoryRoaming; set => Local.Default.DirectoryRoaming = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerFill.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerFill.Description", Resource)]
        public Color ColorMarkerFill { get => Roaming.Default.ColorMarkerFill; set => Roaming.Default.ColorMarkerFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerFillAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerFillAlpha.Description", Resource)]
        public byte ColorMarkerFillAlpha { get => Roaming.Default.ColorMarkerFillAlpha; set => Roaming.Default.ColorMarkerFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerText.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerText.Description", Resource)]
        public Color ColorMarkerText { get => Roaming.Default.ColorMarkerText; set => Roaming.Default.ColorMarkerText = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerTextAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerTextAlpha.Description", Resource)]
        public byte ColorMarkerTextAlpha { get => Roaming.Default.ColorMarkerTextAlpha; set => Roaming.Default.ColorMarkerTextAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFill.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerTextFill.Description", Resource)]
        public Color ColorMarkerTextFill { get => Roaming.Default.ColorMarkerTextFill; set => Roaming.Default.ColorMarkerTextFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFillAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerTextFillAlpha.Description", Resource)]
        public byte ColorMarkerTextFillAlpha { get => Roaming.Default.ColorMarkerTextFillAlpha; set => Roaming.Default.ColorMarkerTextFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerLine.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerLine.Description", Resource)]
        public Color ColorMarkerLine { get => Roaming.Default.ColorMarkerLine; set => Roaming.Default.ColorMarkerLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFill.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerSelectedFill.Description", Resource)]
        public Color ColorMarkerSelectedFill { get => Roaming.Default.ColorMarkerSelectedFill; set => Roaming.Default.ColorMarkerSelectedFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFillAlphaDisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerSelectedFillAlpha.Description", Resource)]
        public byte ColorMarkerSelectedFillAlpha { get => Roaming.Default.ColorMarkerSelectedFillAlpha; set => Roaming.Default.ColorMarkerSelectedFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerLineAlpha.Description", Resource)]
        public byte ColorMarkerLineAlpha { get => Roaming.Default.ColorMarkerLineAlpha; set => Roaming.Default.ColorMarkerLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLine.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerSelectedLine.Description", Resource)]
        public Color ColorMarkerSelectedLine { get => Roaming.Default.ColorMarkerSelectedLine; set => Roaming.Default.ColorMarkerSelectedLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerSelectedLineAlpha.Description", Resource)]
        public byte ColorMarkerSelectedLineAlpha { get => Roaming.Default.ColorMarkerSelectedLineAlpha; set => Roaming.Default.ColorMarkerSelectedLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("ColorMarkerPosition.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorMarkerPosition.Description", Resource)]
        public Color ColorMarkerPosition { get => Roaming.Default.ColorMarkerPosition; set => Roaming.Default.ColorMarkerPosition = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("WidthMarkerLine.DisplayName", Resource)]
        [LocalizedAttribute.Description("WidthMarkerLine.Description", Resource)]
        public int WidthMarkerLine { get => Roaming.Default.WidthMarkerLine; set => Roaming.Default.WidthMarkerLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("WidthMarkerLineSelected.DisplayName", Resource)]
        [LocalizedAttribute.Description("WidthMarkerLineSelected.Description", Resource)]
        public int WidthMarkerLineSelected { get => Roaming.Default.WidthMarkerLineSelected; set => Roaming.Default.WidthMarkerLineSelected = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", Resource)]
        [LocalizedAttribute.DisplayName("FontMarker.DisplayName", Resource)]
        [LocalizedAttribute.Description("FontMarker.Description", Resource)]
        public Font FontMarker { get => Roaming.Default.FontMarker; set => Roaming.Default.FontMarker = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileCluster.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileCluster.Description", Resource)]
        public Color ColorTileCluster { get => Roaming.Default.ColorTileCluster; set => Roaming.Default.ColorTileCluster = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileClusterAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileClusterAlpha.Description", Resource)]
        public byte ColorTileClusterAlpha { get => Roaming.Default.ColorTileClusterAlpha; set => Roaming.Default.ColorTileClusterAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileClusterLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileClusterLineAlpha.Description", Resource)]
        public byte ColorTileClusterLineAlpha { get => Roaming.Default.ColorTileClusterLineAlpha; set => Roaming.Default.ColorTileClusterLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxCluster.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxCluster.Description", Resource)]
        public Color ColorTileMaxCluster { get => Roaming.Default.ColorTileMaxCluster; set => Roaming.Default.ColorTileMaxCluster = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxClusterAlpha.Description", Resource)]
        public byte ColorTileMaxClusterAlpha { get => Roaming.Default.ColorTileMaxClusterAlpha; set => Roaming.Default.ColorTileMaxClusterAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxClusterLineAlpha.Description", Resource)]
        public byte ColorTileMaxClusterLineAlpha { get => Roaming.Default.ColorTileMaxClusterLineAlpha; set => Roaming.Default.ColorTileMaxClusterLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquare.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxSquare.Description", Resource)]
        public Color ColorTileMaxSquare { get => Roaming.Default.ColorTileMaxSquare; set => Roaming.Default.ColorTileMaxSquare = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxSquareAlpha.Description", Resource)]
        public byte ColorTileMaxSquareAlpha { get => Roaming.Default.ColorTileMaxSquareAlpha; set => Roaming.Default.ColorTileMaxSquareAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileMaxSquareLineAlpha.Description", Resource)]
        public byte ColorTileMaxSquareLineAlpha { get => Roaming.Default.ColorTileMaxSquareLineAlpha; set => Roaming.Default.ColorTileMaxSquareLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileVisited.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileVisited.Description", Resource)]
        public Color ColorTileVisited { get => Roaming.Default.ColorTileVisited; set => Roaming.Default.ColorTileVisited = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileVisitedAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileVisitedAlpha.Description", Resource)]
        public byte ColorTileVisitedAlpha { get => Roaming.Default.ColorTileVisitedAlpha; set => Roaming.Default.ColorTileVisitedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileVisitedLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileVisitedLineAlpha.Description", Resource)]
        public byte ColorTileVisitedLineAlpha { get => Roaming.Default.ColorTileVisitedLineAlpha; set => Roaming.Default.ColorTileVisitedLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelected.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileTrackSelected.Description", Resource)]
        public Color ColorTileTrackSelected { get => Roaming.Default.ColorTileTrackSelected; set => Roaming.Default.ColorTileTrackSelected = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileTrackSelectedAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedAlpha { get => Roaming.Default.ColorTileTrackSelectedAlpha; set => Roaming.Default.ColorTileTrackSelectedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileTrackSelectedLineAlpha.Description", Resource)]
        public byte ColorTileTrackSelectedLineAlpha { get => Roaming.Default.ColorTileTrackSelectedLineAlpha; set => Roaming.Default.ColorTileTrackSelectedLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileHeatmap.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileHeatmap.Description", Resource)]
        public Color ColorTileHeatmap { get => Roaming.Default.ColorTileHeatmap; set => Roaming.Default.ColorTileHeatmap = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileHeatmapLineAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileHeatmapLineAlpha.Description", Resource)]
        public byte ColorTileHeatmapLineAlpha { get => Roaming.Default.ColorTileHeatmapLineAlpha; set => Roaming.Default.ColorTileHeatmapLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileHeatmapMinAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileHeatmapMinAlpha.Description", Resource)]
        public byte ColorTileHeatmapMinAlpha { get => Roaming.Default.ColorTileHeatmapMinAlpha; set => Roaming.Default.ColorTileHeatmapMinAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", Resource)]
        [LocalizedAttribute.DisplayName("ColorTileHeatmapMaxAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTileHeatmapMaxAlpha.Description", Resource)]
        public byte ColorTileHeatmapMaxAlpha { get => Roaming.Default.ColorTileHeatmapMaxAlpha; set => Roaming.Default.ColorTileHeatmapMaxAlpha = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("ColorTrack.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTrack.Description", Resource)]
        public Color ColorTrack { get => Roaming.Default.ColorTrack; set => Roaming.Default.ColorTrack = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("ColorTrackAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTrackAlpha.Description", Resource)]
        public byte ColorTrackAlpha { get => Roaming.Default.ColorTrackAlpha; set => Roaming.Default.ColorTrackAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("ColorTrackSelected.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTrackSelected.Description", Resource)]
        public Color ColorTrackSelected { get => Roaming.Default.ColorTrackSelected; set => Roaming.Default.ColorTrackSelected = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("ColorTrackSelectedAlpha.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorTrackSelectedAlpha.Description", Resource)]
        public byte ColorTrackSelectedAlpha { get => Roaming.Default.ColorTrackSelectedAlpha; set => Roaming.Default.ColorTrackSelectedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("WidthTrack.DisplayName", Resource)]
        [LocalizedAttribute.Description("WidthTrack.Description", Resource)]
        public int WidthTrack { get => Roaming.Default.WidthTrack; set => Roaming.Default.WidthTrack = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", Resource)]
        [LocalizedAttribute.DisplayName("WidthTrackSelected.DisplayName", Resource)]
        [LocalizedAttribute.Description("WidthTrackSelected.Description", Resource)]
        public int WidthTrackSelected { get => Roaming.Default.WidthTrackSelected; set => Roaming.Default.WidthTrackSelected = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatDate.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatDate.Description", Resource)]
        public string FormatDate { get => Roaming.Default.FormatDate; set => Roaming.Default.FormatDate = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatTime.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatTime.Description", Resource)]
        public string FormatTime { get => Roaming.Default.FormatTime; set => Roaming.Default.FormatTime = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatDateTime.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatDateTime.Description", Resource)]
        public string FormatDateTime { get => Roaming.Default.FormatDateTime; set => Roaming.Default.FormatDateTime = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatDistance.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatDistance.Description", Resource)]
        public string FormatDistance { get => Roaming.Default.FormatDistance; set => Roaming.Default.FormatDistance = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatDistance2.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatDistance2.Description", Resource)]
        public string FormatDistance2 { get => Roaming.Default.FormatDistance2; set => Roaming.Default.FormatDistance2 = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatEleAscent.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatEleAscent.Description", Resource)]
        public string FormatEleAscent { get => Roaming.Default.FormatEleAscent; set => Roaming.Default.FormatEleAscent = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatEleAscent2.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatEleAscent2.Description", Resource)]
        public string FormatEleAscent2 { get => Roaming.Default.FormatEleAscent2; set => Roaming.Default.FormatEleAscent2 = value; }

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("FormatLatLng.DisplayName", Resource)]
        [LocalizedAttribute.Description("FormatLatLng.Description", Resource)]
        public string FormatLatLng { get => Roaming.Default.FormatLatLng; set => Roaming.Default.FormatLatLng = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Osm", Resource)]
        [LocalizedAttribute.DisplayName("OsmTileKey.DisplayName", Resource)]
        [LocalizedAttribute.Description("OsmTileKey.Description", Resource)]
        public string OsmTileKey { get => Roaming.Default.OsmTileKey; set => Roaming.Default.OsmTileKey = value; }

        [LocalizedAttribute.Category("Category.Osm", Resource)]
        [LocalizedAttribute.DisplayName("OsmTileValue.DisplayName", Resource)]
        [LocalizedAttribute.Description("OsmTileValue.Description", Resource)]
        public string OsmTileValue { get => Roaming.Default.OsmTileValue; set => Roaming.Default.OsmTileValue = value; }

        [LocalizedAttribute.Category("Category.Osm", Resource)]
        [LocalizedAttribute.DisplayName("SaveOsmTileMinZoom.DisplayName", Resource)]
        [LocalizedAttribute.Description("SaveOsmTileMinZoom.Description", Resource)]
        public int SaveOsmTileMinZoom { get => Roaming.Default.SaveOsmTileMinZoom; set => Roaming.Default.SaveOsmTileMinZoom = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.TileStatus", Resource)]
        [LocalizedAttribute.DisplayName("TileStatusFileWptType.DisplayName", Resource)]
        [LocalizedAttribute.Description("TileStatusFileWptType.Description", Resource)]
        public string TileStatusFileWptType { get => Roaming.Default.TileStatusFileWptType; set => Roaming.Default.TileStatusFileWptType = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", Resource)]
        [LocalizedAttribute.DisplayName("TileStatusFileUseOsmand.DisplayName", Resource)]
        [LocalizedAttribute.Description("TileStatusFileUseOsmand.Description", Resource)]
        public bool TileStatusFileUseOsmand { get => Roaming.Default.TileStatusFileUseOsmand; set => Roaming.Default.TileStatusFileUseOsmand = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", Resource)]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIcon.DisplayName", Resource)]
        [LocalizedAttribute.Description("TileStatusFileOsmandIcon.Description", Resource)]
        public string TileStatusFileOsmandIcon { get => Roaming.Default.TileStatusFileOsmandIcon; set => Roaming.Default.TileStatusFileOsmandIcon = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", Resource)]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconBackground.DisplayName", Resource)]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconBackground.Description", Resource)]
        public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get => Roaming.Default.TileStatusFileOsmandIconBackground; set => Roaming.Default.TileStatusFileOsmandIconBackground = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", Resource)]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconColor.DisplayName", Resource)]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconColor.Description", Resource)]
        public Color TileStatusFileOsmandIconColor { get => Roaming.Default.TileStatusFileOsmandIconColor; set => Roaming.Default.TileStatusFileOsmandIconColor = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartSerial.DisplayName", Resource)]
        public Color ColorChartSerial { get => Roaming.Default.ColorChartSerial; set => Roaming.Default.ColorChartSerial = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartSerialAlpha.DisplayName", Resource)]
        public byte ColorChartSerialAlpha { get => Roaming.Default.ColorChartSerialAlpha; set => Roaming.Default.ColorChartSerialAlpha = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartTracksByYearSerial.DisplayName", Resource)]
        public Color ColorChartTracksByYearSerial { get => Roaming.Default.ColorChartTracksByYearSerial; set => Roaming.Default.ColorChartTracksByYearSerial = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartTracksByYearSerialAlpha.DisplayName", Resource)]
        public byte ColorChartTracksByYearSerialAlpha { get => Roaming.Default.ColorChartTracksByYearSerialAlpha; set => Roaming.Default.ColorChartTracksByYearSerialAlpha = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartCursor.DisplayName", Resource)]
        public Color ColorChartCursor { get => Roaming.Default.ColorChartCursor; set => Roaming.Default.ColorChartCursor = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartAxis.DisplayName", Resource)]
        public Color ColorChartAxis { get => Roaming.Default.ColorChartAxis; set => Roaming.Default.ColorChartAxis = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartGrid.DisplayName", Resource)]
        public Color ColorChartGrid { get => Roaming.Default.ColorChartGrid; set => Roaming.Default.ColorChartGrid = value; }

        [LocalizedAttribute.Category("Category.Chart", Resource)]
        [LocalizedAttribute.DisplayName("ColorChartText.DisplayName", Resource)]
        [LocalizedAttribute.Description("ColorChartText.Description", Resource)]
        public Color ColorChartText { get => Roaming.Default.ColorChartText; set => Roaming.Default.ColorChartText = value; }

        // ------------------------------------------------------------------------------------------------------------
        public static Exception LastError { get; private set; } = null;

        // ------------------------------------------------------------------------------------------------------------
        public static bool LocalSave()
        {
            if (!Local.Save())
            {
                LastError = Local.LastError;

                DebugWrite.Error(LastError);

                return false;
            }

            return true;
        }

        public static bool RoamingSave()
        {
            if (!Roaming.Save())
            {
                LastError = Roaming.LastError;

                DebugWrite.Error(LastError);

                return false;
            }

            return true;
        }

        public static bool Save()
        {
            return LocalSave() && RoamingSave();
        }

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

            if (!Local.Load())
            {
                LastError = Local.LastError;

                DebugWrite.Error(LastError);

                return false;
            }

            UpdateDirectoryRoaming();

            return true;
        }

        public static bool RoamingLoad()
        {
            DebugWrite.Line($"Settings Roaming: {Roaming.FilePath}");

            if (!Roaming.Load())
            {
                LastError = Roaming.LastError;

                DebugWrite.Error(LastError);

                return false;
            }

            return true;
        }

        public static bool Load()
        {
            return LocalLoad() && RoamingLoad();
        }

        public static Local.ColumnState[] SaveDataGridColumns(DataGridView dataGridView)
        {
            return Local.SaveDataGridColumns(dataGridView);
        }

        public static void LoadDataGridColumns(DataGridView dataGridView, Local.ColumnState[] columns)
        {
            Local.LoadDataGridColumns(dataGridView, columns);
        }

        public static Local.FormState SaveFormState(Form form)
        {
            return Local.SaveFormState(form);
        }

        public static void LoadFormState(Form form, Local.FormState state)
        {
            Local.LoadFormState(form, state);
        }
    }
}
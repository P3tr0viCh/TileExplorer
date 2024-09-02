using P3tr0viCh.Utils;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        [LocalizedAttribute.Category("Category.Common", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("DatabaseHome.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("DatabaseHome.Description", "Properties.Resources.Settings")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DatabaseHome { get => Local.Default.DatabaseHome; set => Local.Default.DatabaseHome = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerFill { get => Roaming.Default.ColorMarkerFill; set => Roaming.Default.ColorMarkerFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerFillAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerFillAlpha { get => Roaming.Default.ColorMarkerFillAlpha; set => Roaming.Default.ColorMarkerFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerText.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerText.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerText { get => Roaming.Default.ColorMarkerText; set => Roaming.Default.ColorMarkerText = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerTextAlpha { get => Roaming.Default.ColorMarkerTextAlpha; set => Roaming.Default.ColorMarkerTextAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerTextFill { get => Roaming.Default.ColorMarkerTextFill; set => Roaming.Default.ColorMarkerTextFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerTextFillAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerTextFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerTextFillAlpha { get => Roaming.Default.ColorMarkerTextFillAlpha; set => Roaming.Default.ColorMarkerTextFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerLine.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerLine { get => Roaming.Default.ColorMarkerLine; set => Roaming.Default.ColorMarkerLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFill.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedFill.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerSelectedFill { get => Roaming.Default.ColorMarkerSelectedFill; set => Roaming.Default.ColorMarkerSelectedFill = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedFillAlphaDisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedFillAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerSelectedFillAlpha { get => Roaming.Default.ColorMarkerSelectedFillAlpha; set => Roaming.Default.ColorMarkerSelectedFillAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerLineAlpha { get => Roaming.Default.ColorMarkerLineAlpha; set => Roaming.Default.ColorMarkerLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedLine.Description", "Properties.Resources.Settings")]
        public Color ColorMarkerSelectedLine { get => Roaming.Default.ColorMarkerSelectedLine; set => Roaming.Default.ColorMarkerSelectedLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorMarkerSelectedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorMarkerSelectedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorMarkerSelectedLineAlpha { get => Roaming.Default.ColorMarkerSelectedLineAlpha; set => Roaming.Default.ColorMarkerSelectedLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthMarkerLine.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthMarkerLine.Description", "Properties.Resources.Settings")]
        public int WidthMarkerLine { get => Roaming.Default.WidthMarkerLine; set => Roaming.Default.WidthMarkerLine = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthMarkerLineSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthMarkerLineSelected.Description", "Properties.Resources.Settings")]
        public int WidthMarkerLineSelected { get => Roaming.Default.WidthMarkerLineSelected; set => Roaming.Default.WidthMarkerLineSelected = value; }

        [LocalizedAttribute.Category("Category.DesignMarkers", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FontMarker.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FontMarker.Description", "Properties.Resources.Settings")]
        public Font FontMarker { get => Roaming.Default.FontMarker; set => Roaming.Default.FontMarker = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileCluster.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileCluster.Description", "Properties.Resources.Settings")]
        public Color ColorTileCluster { get => Roaming.Default.ColorTileCluster; set => Roaming.Default.ColorTileCluster = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileClusterAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileClusterAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileClusterAlpha { get => Roaming.Default.ColorTileClusterAlpha; set => Roaming.Default.ColorTileClusterAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileClusterLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileClusterLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileClusterLineAlpha { get => Roaming.Default.ColorTileClusterLineAlpha; set => Roaming.Default.ColorTileClusterLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxCluster.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxCluster.Description", "Properties.Resources.Settings")]
        public Color ColorTileMaxCluster { get => Roaming.Default.ColorTileMaxCluster; set => Roaming.Default.ColorTileMaxCluster = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxClusterAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxClusterAlpha { get => Roaming.Default.ColorTileMaxClusterAlpha; set => Roaming.Default.ColorTileMaxClusterAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxClusterLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxClusterLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxClusterLineAlpha { get => Roaming.Default.ColorTileMaxClusterLineAlpha; set => Roaming.Default.ColorTileMaxClusterLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquare.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquare.Description", "Properties.Resources.Settings")]
        public Color ColorTileMaxSquare { get => Roaming.Default.ColorTileMaxSquare; set => Roaming.Default.ColorTileMaxSquare = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquareAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxSquareAlpha { get => Roaming.Default.ColorTileMaxSquareAlpha; set => Roaming.Default.ColorTileMaxSquareAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileMaxSquareLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileMaxSquareLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileMaxSquareLineAlpha { get => Roaming.Default.ColorTileMaxSquareLineAlpha; set => Roaming.Default.ColorTileMaxSquareLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisited.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisited.Description", "Properties.Resources.Settings")]
        public Color ColorTileVisited { get => Roaming.Default.ColorTileVisited; set => Roaming.Default.ColorTileVisited = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisitedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisitedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileVisitedAlpha { get => Roaming.Default.ColorTileVisitedAlpha; set => Roaming.Default.ColorTileVisitedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileVisitedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileVisitedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileVisitedLineAlpha { get => Roaming.Default.ColorTileVisitedLineAlpha; set => Roaming.Default.ColorTileVisitedLineAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelected.Description", "Properties.Resources.Settings")]
        public Color ColorTileTrackSelected { get => Roaming.Default.ColorTileTrackSelected; set => Roaming.Default.ColorTileTrackSelected = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelectedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileTrackSelectedAlpha { get => Roaming.Default.ColorTileTrackSelectedAlpha; set => Roaming.Default.ColorTileTrackSelectedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTiles", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTileTrackSelectedLineAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTileTrackSelectedLineAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTileTrackSelectedLineAlpha { get => Roaming.Default.ColorTileTrackSelectedLineAlpha; set => Roaming.Default.ColorTileTrackSelectedLineAlpha = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrack.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrack.Description", "Properties.Resources.Settings")]
        public Color ColorTrack { get => Roaming.Default.ColorTrack; set => Roaming.Default.ColorTrack = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTrackAlpha { get => Roaming.Default.ColorTrackAlpha; set => Roaming.Default.ColorTrackAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackSelected.Description", "Properties.Resources.Settings")]
        public Color ColorTrackSelected { get => Roaming.Default.ColorTrackSelected; set => Roaming.Default.ColorTrackSelected = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("ColorTrackSelectedAlpha.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("ColorTrackSelectedAlpha.Description", "Properties.Resources.Settings")]
        public byte ColorTrackSelectedAlpha { get => Roaming.Default.ColorTrackSelectedAlpha; set => Roaming.Default.ColorTrackSelectedAlpha = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthTrack.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthTrack.Description", "Properties.Resources.Settings")]
        public int WidthTrack { get => Roaming.Default.WidthTrack; set => Roaming.Default.WidthTrack = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("WidthTrackSelected.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("WidthTrackSelected.Description", "Properties.Resources.Settings")]
        public int WidthTrackSelected { get => Roaming.Default.WidthTrack; set => Roaming.Default.WidthTrack = value; }

        [LocalizedAttribute.Category("Category.DesignTracks", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TrackMinDistancePoint.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TrackMinDistancePoint.Description", "Properties.Resources.Settings")]
        public int TrackMinDistancePoint { get => Roaming.Default.TrackMinDistancePoint; set => Roaming.Default.TrackMinDistancePoint = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDate.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDate.Description", "Properties.Resources.Settings")]
        public string FormatDate { get => Roaming.Default.FormatDate; set => Roaming.Default.FormatDate = value; }

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatTime.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatTime.Description", "Properties.Resources.Settings")]
        public string FormatTime { get => Roaming.Default.FormatTime; set => Roaming.Default.FormatTime = value; }

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDateTime.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDateTime.Description", "Properties.Resources.Settings")]
        public string FormatDateTime { get => Roaming.Default.FormatDateTime; set => Roaming.Default.FormatDateTime = value; }

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDistance.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDistance.Description", "Properties.Resources.Settings")]
        public string FormatDistance { get => Roaming.Default.FormatDistance; set => Roaming.Default.FormatDistance = value; }
        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatDistance2.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatDistance2.Description", "Properties.Resources.Settings")]
        public string FormatDistance2 { get => Roaming.Default.FormatDistance2; set => Roaming.Default.FormatDistance2 = value; }

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatEleAscent.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatEleAscent.Description", "Properties.Resources.Settings")]
        public string FormatEleAscent { get => Roaming.Default.FormatEleAscent; set => Roaming.Default.FormatEleAscent = value; }

        [LocalizedAttribute.Category("Category.Format", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("FormatLatLng.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("FormatLatLng.Description", "Properties.Resources.Settings")]
        public string FormatLatLng { get => Roaming.Default.FormatLatLng; set => Roaming.Default.FormatLatLng = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("OsmTileKey.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("OsmTileKey.Description", "Properties.Resources.Settings")]
        public string OsmTileKey { get => Roaming.Default.OsmTileKey; set => Roaming.Default.OsmTileKey = value; }

        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("OsmTileValue.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("OsmTileValue.Description", "Properties.Resources.Settings")]
        public string OsmTileValue { get => Roaming.Default.OsmTileValue; set => Roaming.Default.OsmTileValue = value; }

        [LocalizedAttribute.Category("Category.Osm", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("SaveOsmTileMinZoom.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("SaveOsmTileMinZoom.Description", "Properties.Resources.Settings")]
        public int SaveOsmTileMinZoom { get => Roaming.Default.SaveOsmTileMinZoom; set => Roaming.Default.SaveOsmTileMinZoom = value; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.TileStatus", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileWptType.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileWptType.Description", "Properties.Resources.Settings")]
        public string TileStatusFileWptType { get => Roaming.Default.TileStatusFileWptType; set => Roaming.Default.TileStatusFileWptType = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileUseOsmand.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileUseOsmand.Description", "Properties.Resources.Settings")]
        public bool TileStatusFileUseOsmand { get => Roaming.Default.TileStatusFileUseOsmand; set => Roaming.Default.TileStatusFileUseOsmand = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIcon.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIcon.Description", "Properties.Resources.Settings")]
        public string TileStatusFileOsmandIcon { get => Roaming.Default.TileStatusFileOsmandIcon; set => Roaming.Default.TileStatusFileOsmandIcon = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconBackground.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconBackground.Description", "Properties.Resources.Settings")]
        public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get => Roaming.Default.TileStatusFileOsmandIconBackground; set => Roaming.Default.TileStatusFileOsmandIconBackground = value; }

        [LocalizedAttribute.Category("Category.TileStatusOsmand", "Properties.Resources.Settings")]
        [LocalizedAttribute.DisplayName("TileStatusFileOsmandIconColor.DisplayName", "Properties.Resources.Settings")]
        [LocalizedAttribute.Description("TileStatusFileOsmandIconColor.Description", "Properties.Resources.Settings")]
        public Color TileStatusFileOsmandIconColor { get => Roaming.Default.TileStatusFileOsmandIconColor; set => Roaming.Default.TileStatusFileOsmandIconColor = value; }

        // ------------------------------------------------------------------------------------------------------------
        public static void LocalSave()
        {
            if (!Local.Save())
            {
                DebugWrite.Error(Local.LastError);
            }
        }

        public static void RoamingSave()
        {
            if (!Roaming.Save())
            {
                DebugWrite.Error(Roaming.LastError);
            }
        }

        public static void Save()
        {
            LocalSave();
            RoamingSave();
        }

        public static void LocalLoad()
        {
            if (!Local.Load())
            {
                DebugWrite.Error(Local.LastError);
            }
        }

        public static void RoamingLoad()
        {
            if (!Roaming.Load())
            {
                DebugWrite.Error(Roaming.LastError);
            }
        }

        public static void Load()
        {
            LocalLoad();
            RoamingLoad();
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
﻿using GMap.NET;
using P3tr0viCh.Utils;
using System.Drawing;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal partial class AppSettings
    {
        internal class Roaming : SettingsBase<Roaming>
        {
            // ------------------------------------------------------------------------------------------------------------
            public MouseWheelZoomType MouseWheelZoomType { get; set; } = MouseWheelZoomType.MousePositionWithoutCenter;

            // ------------------------------------------------------------------------------------------------------------
            public Color ColorMarkerFill { get; set; } = Color.FromArgb(44, 165, 126);

            public byte ColorMarkerFillAlpha { get; set; } = 220;

            public Color ColorMarkerText { get; set; } = Color.FromArgb(44, 165, 126);

            public byte ColorMarkerTextAlpha { get; set; } = 255;

            public Color ColorMarkerTextFill { get; set; } = Color.LightGray;

            public byte ColorMarkerTextFillAlpha { get; set; } = 220;

            public Color ColorMarkerLine { get; set; } = Color.Gray;

            public Color ColorMarkerSelectedFill { get; set; } = Color.DarkSlateBlue;

            public byte ColorMarkerSelectedFillAlpha { get; set; } = 220;

            public byte ColorMarkerLineAlpha { get; set; } = 160;

            public Color ColorMarkerSelectedLine { get; set; } = Color.DarkSlateBlue;

            public byte ColorMarkerSelectedLineAlpha { get; set; } = 220;

            public Color ColorMarkerPosition { get; set; } = Color.FromArgb(59, 167, 199);

            public int WidthMarkerLine { get; set; } = 1;

            public int WidthMarkerLineSelected { get; set; } = 2;

            public Font FontMarker { get; set; } = new Font("Arial", 10);

            // ------------------------------------------------------------------------------------------------------------
            public Color ColorTileCluster { get; set; } = Color.FromArgb(220, 220, 20);

            public byte ColorTileClusterAlpha { get; set; } = 40;

            public byte ColorTileClusterLineAlpha { get; set; } = 50;

            public Color ColorTileMaxCluster { get; set; } = Color.FromArgb(44, 165, 126);

            public byte ColorTileMaxClusterAlpha { get; set; } = 40;

            public byte ColorTileMaxClusterLineAlpha { get; set; } = 50;

            public Color ColorTileMaxSquare { get; set; } = Color.FromArgb(44, 165, 126);

            public byte ColorTileMaxSquareAlpha { get; set; } = 60;

            public byte ColorTileMaxSquareLineAlpha { get; set; } = 90;

            public Color ColorTileVisited { get; set; } = Color.Red;

            public byte ColorTileVisitedAlpha { get; set; } = 25;

            public byte ColorTileVisitedLineAlpha { get; set; } = 25;

            public Color ColorTileTrackSelected { get; set; } = Color.DarkSlateBlue;

            public byte ColorTileTrackSelectedAlpha { get; set; } = 144;

            public byte ColorTileTrackSelectedLineAlpha { get; set; } = 88;

            public Color ColorTileHeatmap { get; set; } = Color.Red;

            public byte ColorTileHeatmapLineAlpha { get; set; } = 25;

            public byte ColorTileHeatmapMinAlpha { get; set; } = 10;
            public byte ColorTileHeatmapMaxAlpha { get; set; } = 130;

            // ------------------------------------------------------------------------------------------------------------
            public Color ColorTrack { get; set; } = Color.Red;

            public byte ColorTrackAlpha { get; set; } = 144;

            public Color ColorTrackSelected { get; set; } = Color.DarkSlateBlue;

            public byte ColorTrackSelectedAlpha { get; set; } = 220;

            public int WidthTrack { get; set; } = 2;

            public int WidthTrackSelected { get; set; } = 2;

            // ------------------------------------------------------------------------------------------------------------
            public string FormatDate { get; set; } = "yyyy.MM.dd";

            public string FormatTime { get; set; } = "HH:mm";

            public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

            public string FormatDistance { get; set; } = "0.00";

            public string FormatDistance2 { get; set; } = "0";

            public string FormatEleAscent { get; set; } = "0";

            public string FormatLatLng { get; set; } = "0.000000";

            // ------------------------------------------------------------------------------------------------------------
            public string OsmTileKey { get; set; } = "boundary";

            public string OsmTileValue { get; set; } = "tile";

            public int SaveOsmTileMinZoom { get; set; } = 10;

            // ------------------------------------------------------------------------------------------------------------
            public string TileStatusFileWptType { get; set; } = string.Empty;

            public bool TileStatusFileUseOsmand { get; set; } = true;

            public string TileStatusFileOsmandIcon { get; set; } = "special_flag_stroke";

            public OsmandIconBackgroud TileStatusFileOsmandIconBackground { get; set; } = OsmandIconBackgroud.Circle;

            public Color TileStatusFileOsmandIconColor { get; set; } = Color.Red;

            // ------------------------------------------------------------------------------------------------------------
            public Color ColorChartSerial { get; set; } = Color.FromArgb(217, 217, 217);
            public byte ColorChartSerialAlpha { get; set; } = 192;

            public Color ColorChartTracksByYearSerial { get; set; } = Color.Black;
            public byte ColorChartTracksByYearSerialAlpha { get; set; } = 192;

            public Color ColorChartCursor { get; set; } = Color.Black;

            public Color ColorChartAxis { get; set; } = Color.FromArgb(170, 170, 170);

            public Color ColorChartGrid { get; set; } = Color.FromArgb(217, 217, 217);

            public Color ColorChartText { get; set; } = Color.FromArgb(80, 80, 80);
        }
    }
}
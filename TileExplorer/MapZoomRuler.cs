using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Drawing;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class MapZoomRuler
    {
        private const int rulerMinZoom = 6;

        private const int rulerPositionLeft = 10;
        private const int rulerPositionBottom = 20;

        private const int textMarginLeft = 5;
        private const int textMarginTop = 1;
        private const int textMarginBottom = 1;

        private const int penLineWidth = 2;

        private Rectangle rulerBounds = new Rectangle();

        private readonly SolidBrush brushBack = new SolidBrush(Color.FromArgb((int)(255 * 0.8), 255, 255, 255));

        private readonly Pen penLine = new Pen(Color.FromArgb(255, 119, 119, 119), penLineWidth);

        private readonly Brush brushText = new SolidBrush(Color.FromArgb(255, 119, 119, 119));

        private readonly GMapControl parent;

        private static readonly int[] zoomMeters = {
            5000000, 3000000, 2000000, 1000000, 500000, 300000,
            100000, 50000, 30000, 10000, 5000, 3000, 2000, 1000,
            500, 200, 100, 50, 30, 10 };

        public MapZoomRuler(GMapControl parent)
        {
            this.parent = parent;
        }

        public void Paint(Graphics graphics)
        {
            var zoom = (int)parent.Zoom;

            if (zoom < rulerMinZoom)
            {
                return;
            }

            string text;

            if (zoomMeters[zoom] > 1000)
            {
                text = (zoomMeters[zoom] / 1000).ToString() + Str.Space + Resources.TextMapZoomRulerKM;
            }
            else
            {
                text = zoomMeters[zoom].ToString() + Str.Space + Resources.TextMapZoomRulerM;
            }

            rulerBounds.Height = (int)graphics.MeasureString(text, parent.Font).Height +
                textMarginTop + textMarginBottom;

            rulerBounds.X = parent.Bounds.Left + rulerPositionLeft;
            rulerBounds.Y = parent.Bounds.Bottom - rulerPositionBottom - rulerBounds.Height;

            var mapCenter = parent.Position;

            var lngLength = Geo.LongitudeLenghtForLatitude(mapCenter.Lat);

            var x = lngLength * zoomMeters[zoom];

            var p1 = parent.FromLatLngToLocal(mapCenter);

            var p2 = parent.FromLatLngToLocal(new PointLatLng(mapCenter.Lat, mapCenter.Lng + x));

            rulerBounds.Width = (int)Math.Abs(p1.X - p2.X);

            graphics.FillRectangle(brushBack, rulerBounds);

            graphics.DrawLine(penLine, rulerBounds.Left, rulerBounds.Top, rulerBounds.Left, rulerBounds.Bottom);
            graphics.DrawLine(penLine, rulerBounds.Left, rulerBounds.Bottom, rulerBounds.Right, rulerBounds.Bottom);
            graphics.DrawLine(penLine, rulerBounds.Right, rulerBounds.Bottom, rulerBounds.Right, rulerBounds.Top);

            graphics.DrawString(text, parent.Font, brushText, 
                rulerBounds.Left + textMarginLeft, rulerBounds.Top + textMarginTop);
        }
    }
}
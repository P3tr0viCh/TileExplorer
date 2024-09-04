using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Drawing;
using System.Linq;
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
        private const int penLineWidthDraw = penLineWidth / 2;

        private Point rulerPosition = new Point();

        private readonly SolidBrush brushBack = new SolidBrush(Color.FromArgb((int)(255 * 0.8), 255, 255, 255));

        private readonly Pen penLine = new Pen(Color.FromArgb(255, 119, 119, 119), penLineWidth);

        private readonly Brush brushText = new SolidBrush(Color.FromArgb(255, 119, 119, 119));

        private readonly GMapControl parent;

        private static readonly int[] zoomMeters = {
            5000000, 3000000, 2000000, 1000000, 500000, 300000,
            100000, 50000, 30000, 10000, 5000, 3000, 2000, 1000,
            500, 200, 100, 50, 30, 10 };

        private Bitmap bitmap;

        public MapZoomRuler(GMapControl parent)
        {
            this.parent = parent;
        }

        public void Measure()
        {
            var zoom = (int)parent.Zoom;

            if (zoom < rulerMinZoom)
            {
                bitmap = new Bitmap(1, 1);
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

            var height = (int)parent.CreateGraphics().MeasureString(text, parent.Font).Height +
                textMarginTop + textMarginBottom + penLineWidthDraw;

            rulerPosition.X = parent.Bounds.Left + rulerPositionLeft;
            rulerPosition.Y = parent.Bounds.Bottom - rulerPositionBottom - height;

            var mapCenter = parent.Position;

            var meter = 1 / (111319.44 * Math.Cos(Geo.DegToRad(mapCenter.Lat)));

            var x = meter * zoomMeters[zoom];

            var p1 = parent.FromLatLngToLocal(mapCenter);

            var p2 = parent.FromLatLngToLocal(new PointLatLng(mapCenter.Lat, mapCenter.Lng + x));

            var width = (int)Math.Abs(p1.X - p2.X);

            bitmap = new Bitmap(width, height);

            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(brushBack, 0, 0, width, height);

            graphics.DrawLines(penLine, new Point[] {
                new Point(penLineWidthDraw, 0),
                new Point(penLineWidthDraw, height - penLineWidthDraw),
                new Point(width - penLineWidthDraw, height - penLineWidthDraw),
                new Point(width - penLineWidthDraw, 0)
            }.ToArray());

            graphics.DrawString(text, parent.Font, brushText, textMarginLeft, textMarginTop);
        }

        public void Paint(Graphics graphics)
        {
            graphics.DrawImage(bitmap, rulerPosition.X, rulerPosition.Y);
        }
    }
}
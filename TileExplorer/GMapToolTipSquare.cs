using GMap.NET.WindowsForms;
using System.Drawing;

namespace TileExplorer
{
    public class GMapToolTipSquare : GMapToolTip
    {
        private const int MARKER_OFFSET_X = 20;
        private const int MARKER_OFFSET_Y = -30;

        public GMapToolTipSquare(GMapMarker marker) : base(marker)
        {
        }

        public override void OnRender(Graphics g)
        {
            Size size = g.MeasureString(Marker.ToolTipText, Font).ToSize();

            checked
            {
                Rectangle rectangle = new Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - size.Height,
                    size.Width + TextPadding.Width, size.Height + TextPadding.Height);

                rectangle.Offset(MARKER_OFFSET_X + Offset.X, MARKER_OFFSET_Y - Offset.Y);

                g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y,
                    rectangle.X, rectangle.Y + unchecked(rectangle.Height / 2));

                g.FillRectangle(Fill, rectangle);

                g.DrawRectangle(Stroke, rectangle);

                g.DrawString(Marker.ToolTipText, Font, Foreground, rectangle, Format);
            }
        }
    }
}
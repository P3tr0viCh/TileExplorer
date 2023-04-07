using GMap.NET.WindowsForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class MapMarkerToolTip : GMapToolTip
    {
        private const int MARKER_OFFSET_X = 20;
        private const int MARKER_OFFSET_Y = -10;

        static MapMarkerToolTip()
        {
            ((SolidBrush)DefaultFill).Color = Color.FromArgb(
                    Settings.Default.ColorMarkerTextFillAlpha, Settings.Default.ColorMarkerTextFill);

            ((SolidBrush)DefaultForeground).Color = Color.FromArgb(
                Settings.Default.ColorMarkerTextAlpha, Settings.Default.ColorMarkerText);
        }

        public MapMarkerToolTip(MapMarker marker) : base(marker)
        {
            Font = Settings.Default.FontMarker;

            TextPadding = new Size(4, 4);
        }

        public override void OnRender(Graphics g)
        {
            Size size = g.MeasureString(Marker.ToolTipText, Font).ToSize();

            checked
            {
                Rectangle rectangle = new Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - size.Height,
                    size.Width + TextPadding.Width, size.Height + TextPadding.Height);

                rectangle.Offset(MARKER_OFFSET_X + Offset.X, MARKER_OFFSET_Y - Offset.Y);

                int x2;
                int y2;

                if (rectangle.X < Marker.ToolTipPosition.X)
                {
                    if (rectangle.X + rectangle.Width > Marker.ToolTipPosition.X)
                    {
                        x2 = rectangle.X + unchecked(rectangle.Width / 2);

                        if (rectangle.Y < Marker.ToolTipPosition.Y)
                        {
                            y2 = rectangle.Y + rectangle.Height;
                        }
                        else
                        {
                            y2 = rectangle.Y;
                        }
                    }
                    else
                    {
                        x2 = rectangle.X + rectangle.Width;

                        y2 = rectangle.Y + unchecked(rectangle.Height / 2);
                    }
                }
                else
                {
                    if (rectangle.X == Marker.ToolTipPosition.X)
                    {
                        x2 = rectangle.X + unchecked(rectangle.Width / 2);

                        if (rectangle.Y < Marker.ToolTipPosition.Y)
                        {
                            y2 = rectangle.Y + rectangle.Height;
                        }
                        else
                        {
                            y2 = rectangle.Y;
                        }
                    }
                    else
                    {
                        x2 = rectangle.X;

                        y2 = rectangle.Y + unchecked(rectangle.Height / 2);
                    }
                }

                g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, x2, y2);

                g.FillRectangle(Fill, rectangle);

                g.DrawRectangle(Stroke, rectangle);

                g.DrawString(Marker.ToolTipText, Font, Foreground, rectangle, Format);
            }
        }
    }
}
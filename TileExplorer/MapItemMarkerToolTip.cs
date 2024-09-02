using GMap.NET.WindowsForms;
using System.Drawing;

namespace TileExplorer
{
    internal class MapItemMarkerToolTip : GMapToolTip
    {
        public MapItemMarkerToolTip(MapItemMarker marker) : base(marker)
        {
            ((SolidBrush)DefaultFill).Color = Color.FromArgb(
                   AppSettings.Roaming.Default.ColorMarkerTextFillAlpha, AppSettings.Roaming.Default.ColorMarkerTextFill);

            ((SolidBrush)DefaultForeground).Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerTextAlpha, AppSettings.Roaming.Default.ColorMarkerText);

            Font = AppSettings.Roaming.Default.FontMarker;

            TextPadding = new Size(4, 4);

            Offset = new Point(marker.Offset.X, marker.Offset.Y);
        }

        public override void OnRender(Graphics g)
        {
            var size = g.MeasureString(Marker.ToolTipText, Font).ToSize();

            checked
            {
                var rectangle = new Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - size.Height,
                    size.Width + TextPadding.Width, size.Height + TextPadding.Height);

                rectangle.Offset(Offset.X, Offset.Y);

                int x;
                int y;

                if (rectangle.X < Marker.ToolTipPosition.X)
                {
                    if (rectangle.X + rectangle.Width > Marker.ToolTipPosition.X)
                    {
                        x = rectangle.X + unchecked(rectangle.Width / 2);

                        if (rectangle.Y < Marker.ToolTipPosition.Y)
                        {
                            y = rectangle.Y + rectangle.Height;
                        }
                        else
                        {
                            y = rectangle.Y;
                        }
                    }
                    else
                    {
                        x = rectangle.X + rectangle.Width;

                        y = rectangle.Y + unchecked(rectangle.Height / 2);
                    }
                }
                else
                {
                    if (rectangle.X == Marker.ToolTipPosition.X)
                    {
                        x = rectangle.X + unchecked(rectangle.Width / 2);

                        if (rectangle.Y < Marker.ToolTipPosition.Y)
                        {
                            y = rectangle.Y + rectangle.Height;
                        }
                        else
                        {
                            y = rectangle.Y;
                        }
                    }
                    else
                    {
                        x = rectangle.X;

                        y = rectangle.Y + unchecked(rectangle.Height / 2);
                    }
                }

                g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, x, y);

                g.FillRectangle(Fill, rectangle);

                g.DrawRectangle(Stroke, rectangle);

                g.DrawString(Marker.ToolTipText, Font, Foreground, rectangle, Format);
            }
        }
    }
}
using P3tr0viCh.Utils;
using System;
using System.Windows.Forms;
using TileExplorer.Properties;
namespace TileExplorer
{
    internal class PresenterStatusStripChartTrackEle
    {
        public enum StatusLabel
        {
            Ele,
            Distance,
            DateTime,
            DateTimeSpan,
            IsSelection,
        }

        public interface IStatusStripView
        {
            ToolStripStatusLabel GetLabel(StatusLabel label);
        }

        private readonly IStatusStripView view;

        public PresenterStatusStripChartTrackEle(IStatusStripView view)
        {
            this.view = view;

            Ele = 0;
            Distance = 0;
            DateTime = default;

            IsSelection = false;
        }

        public double Ele
        {
            set
            {
                view.GetLabel(StatusLabel.Ele).Text = string.Format(Resources.StatusEle, value);
            }
        }

        public double Distance
        {
            set
            {
                view.GetLabel(StatusLabel.Distance).Text = string.Format(Resources.StatusDistance, value / 1000);
            }
        }

        public DateTime DateTime
        {
            set
            {
                view.GetLabel(StatusLabel.DateTime).Text = value.ToString(AppSettings.Roaming.Default.FormatDateTime);
            }
        }

        public TimeSpan DateTimeSpan
        {
            set
            {
                if (value != default)
                {
                    view.GetLabel(StatusLabel.DateTimeSpan).Text = value.ToHoursMinutesString();
                }
                else
                {
                    view.GetLabel(StatusLabel.DateTimeSpan).Text = string.Empty;
                }
            }
        }

        public bool IsSelection
        {
            set
            {
                view.GetLabel(StatusLabel.IsSelection).Text = value ? Resources.StatusIsSelection : string.Empty;
            }
        }
    }
}
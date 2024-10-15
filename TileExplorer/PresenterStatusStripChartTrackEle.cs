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
            SelectedDistance,
            SelectedEleAscent,
        }

        public interface IPresenterStatusStripChartTrackEle
        {
            ToolStripStatusLabel GetLabel(StatusLabel label);
        }

        private readonly IPresenterStatusStripChartTrackEle view;

        public PresenterStatusStripChartTrackEle(IPresenterStatusStripChartTrackEle view)
        {
            this.view = view;

            Ele = 0;
            Distance = 0;
            DateTime = default;

            IsSelection = false;

            SelectedEleAscent = 0;
            SelectedDistance = 0;
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

                view.GetLabel(StatusLabel.SelectedDistance).Visible = value;
                view.GetLabel(StatusLabel.SelectedEleAscent).Visible = value;
            }
        }

        public double SelectedEleAscent
        {
            set
            {
                view.GetLabel(StatusLabel.SelectedEleAscent).Text = string.Format(Resources.StatusEleAscent, value);
            }
        }

        public double SelectedDistance
        {
            set
            {
                view.GetLabel(StatusLabel.SelectedDistance).Text = string.Format(Resources.StatusDistance, value / 1000);
            }
        }
    }
}
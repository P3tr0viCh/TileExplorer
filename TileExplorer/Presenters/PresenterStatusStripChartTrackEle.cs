using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Presenters;
using System;
using TileExplorer.Properties;

namespace TileExplorer.Presenters
{
    internal class PresenterStatusStripChartTrackEle : PresenterStatusStrip<PresenterStatusStripChartTrackEle.StatusLabel>
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
            SelectedEleDescent,
        }

        public PresenterStatusStripChartTrackEle(IPresenterStatusStrip view) : base(view)
        {
            Ele = 0;
            Distance = 0;
            DateTime = default;

            IsSelection = false;

            SelectedEleAscent = 0;
            SelectedEleDescent = 0;
            SelectedDistance = 0;
        }

        public double Ele
        {
            set => View.GetLabel(StatusLabel.Ele).Text = string.Format(Resources.StatusEle, value);
        }

        public double Distance
        {
            set => View.GetLabel(StatusLabel.Distance).Text = string.Format(Resources.StatusDistance, value / 1000);
        }

        public DateTime DateTime
        {
            set => View.GetLabel(StatusLabel.DateTime).Text = value.ToString(AppSettings.Roaming.Default.FormatDateTime);
        }

        public TimeSpan DateTimeSpan
        {
            set
            {
                if (value != default)
                {
                    View.GetLabel(StatusLabel.DateTimeSpan).Text = value.ToHoursMinutesString();
                }
                else
                {
                    View.GetLabel(StatusLabel.DateTimeSpan).Text = string.Empty;
                }
            }
        }

        public bool IsSelection
        {
            set
            {
                View.GetLabel(StatusLabel.IsSelection).Text = value ? Resources.StatusIsSelection : string.Empty;

                View.GetLabel(StatusLabel.SelectedDistance).Visible = value;
                View.GetLabel(StatusLabel.SelectedEleAscent).Visible = value;
                View.GetLabel(StatusLabel.SelectedEleDescent).Visible = value;
            }
        }

        public double SelectedEleAscent
        {
            set => View.GetLabel(StatusLabel.SelectedEleAscent).Text = string.Format(Resources.StatusEleAscent, value);
        }

        public double SelectedEleDescent
        {
            set => View.GetLabel(StatusLabel.SelectedEleDescent).Text = string.Format(Resources.StatusEleDescent, value);
        }

        public double SelectedDistance
        {
            set => View.GetLabel(StatusLabel.SelectedDistance).Text = string.Format(Resources.StatusDistance, value / 1000);
        }
    }
}
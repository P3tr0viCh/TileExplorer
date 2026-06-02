using System.Windows.Forms;

namespace TileExplorer
{
    public static class DataGridViewCellStyles
    {
        private class TopRight : DataGridViewCellStyle
        {
            public TopRight()
            {
                Alignment = DataGridViewContentAlignment.TopRight;
            }
        }

        private class TopCenter : DataGridViewCellStyle
        {
            public TopCenter()
            {
                Alignment = DataGridViewContentAlignment.TopCenter;
            }
        }
        public static readonly DataGridViewCellStyle Year = new DataGridViewCellStyle()
        {
            Format = "####"
        };

        public static readonly DataGridViewCellStyle Count = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle Distance = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle Speed = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle DistanceSum = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle DistanceStep = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle EleAscentSum = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle EleAscentStep = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle EleAscent = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle LatLng = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle DurationAsString = new TopRight()
        {
        };

        public static readonly DataGridViewCellStyle Date = new DataGridViewCellStyle()
        {
        };

        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
        {
        };

        public static readonly DataGridViewCellStyle State = new TopCenter()
        {
        };

        public static void UpdateSettings()
        {
            Distance.Format = AppSettings.Roaming.Default.FormatDistance;
            DistanceSum.Format = AppSettings.Roaming.Default.FormatDistanceRound;

            EleAscent.Format = AppSettings.Roaming.Default.FormatEleAscent;
            EleAscentSum.Format = AppSettings.Roaming.Default.FormatEleAscentRound;

            LatLng.Format = AppSettings.Roaming.Default.FormatLatLng;

            Date.Format = AppSettings.Roaming.Default.FormatDate;
            DateTime.Format = AppSettings.Roaming.Default.FormatDateTime;

            Speed.Format = AppSettings.Roaming.Default.FormatSpeed;
        }
    }
}
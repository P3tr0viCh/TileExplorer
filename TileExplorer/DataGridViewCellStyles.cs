using System.Windows.Forms;

namespace TileExplorer
{
    public static class DataGridViewCellStyles
    {
        public static readonly DataGridViewCellStyle Year = new DataGridViewCellStyle()
        {
            Format = "####"
        };

        public static readonly DataGridViewCellStyle Count = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight
        };

        public static readonly DataGridViewCellStyle Distance = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle Speed = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = "0.0"
        };

        public static readonly DataGridViewCellStyle DistanceSum = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle DistanceStep = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle EleAscentSum = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle EleAscentStep = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle EleAscent = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle LatLng = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
        };

        public static readonly DataGridViewCellStyle DurationAsString = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight
        };

        public static readonly DataGridViewCellStyle Date = new DataGridViewCellStyle()
        {
        };

        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
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
        }
    }
}
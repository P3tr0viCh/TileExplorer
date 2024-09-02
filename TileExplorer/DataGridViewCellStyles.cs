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
            Format = AppSettings.Roaming.Default.FormatDistance
        };

        public static readonly DataGridViewCellStyle Speed = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = "0.0"
        };

        public static readonly DataGridViewCellStyle DistanceSum = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Roaming.Default.FormatDistance2
        };

        public static readonly DataGridViewCellStyle DistanceStep = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Roaming.Default.FormatDistance2
        };

        public static readonly DataGridViewCellStyle EleAscent = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Roaming.Default.FormatEleAscent
        };

        public static readonly DataGridViewCellStyle LatLng = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Roaming.Default.FormatLatLng
        };

        public static readonly DataGridViewCellStyle DurationAsString = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight
        };

        public static readonly DataGridViewCellStyle Date = new DataGridViewCellStyle()
        {
            Format = AppSettings.Roaming.Default.FormatDate
        };

        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
        {
            Format = AppSettings.Roaming.Default.FormatDateTime
        };

        public static void UpdateSettings()
        {
            Distance.Format = AppSettings.Roaming.Default.FormatDistance;
            DistanceSum.Format = AppSettings.Roaming.Default.FormatDistance2;

            LatLng.Format = AppSettings.Roaming.Default.FormatLatLng;

            Date.Format = AppSettings.Roaming.Default.FormatDate;
            DateTime.Format = AppSettings.Roaming.Default.FormatDateTime;
        }
    }
}
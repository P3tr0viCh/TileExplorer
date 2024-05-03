using System.Windows.Forms;
using TileExplorer.Properties;

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
            Format = AppSettings.Default.FormatDistance
        };

        public static readonly DataGridViewCellStyle DistanceSum = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Default.FormatDistance2
        };

        public static readonly DataGridViewCellStyle EleAscent = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Default.FormatEleAscent
        };

        public static readonly DataGridViewCellStyle LatLng = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight,
            Format = AppSettings.Default.FormatLatLng
        };

        public static readonly DataGridViewCellStyle DurationAsString = new DataGridViewCellStyle()
        {
            Alignment = DataGridViewContentAlignment.TopRight
        };

        public static readonly DataGridViewCellStyle Date = new DataGridViewCellStyle()
        {
            Format = AppSettings.Default.FormatDate
        };

        public static readonly DataGridViewCellStyle DateTime = new DataGridViewCellStyle()
        {
            Format = AppSettings.Default.FormatDateTime
        };

        public static void UpdateSettings()
        {
            Distance.Format = AppSettings.Default.FormatDistance;
            DistanceSum.Format = AppSettings.Default.FormatDistance2;

            LatLng.Format = AppSettings.Default.FormatLatLng;

            Date.Format = AppSettings.Default.FormatDate;
            DateTime.Format = AppSettings.Default.FormatDateTime;
        }
    }
}
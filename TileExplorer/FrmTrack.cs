using System.Diagnostics;
using System;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmTrack : Form
    {
        public FrmTrack()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner, TrackModel track)
        {
            bool Result;

            using (var frm = new FrmTrack())
            {
                frm.tbText.Text = track.Text;
                frm.dtpDateTime.Value = track.DateTime;

                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    track.Text = frm.tbText.Text;
                    track.DateTime = frm.dtpDateTime.Value;
                }
            }

            return Result;
        }

        private TimeSpan UtcOffset
        {
            get
            {
                return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            }
        }

        private void FrmTrack_Load(object sender, EventArgs e)
        {
            dtpDateTime.CustomFormat = Settings.Default.FormatDateTime;

            miDateTimeAddTimeZone.Text = string.Format(miDateTimeAddTimeZone.Text, UtcOffset.TotalHours);
        }

        private void MiDateTimeAddTimeZone_Click(object sender, EventArgs e)
        {
            dtpDateTime.Value += UtcOffset;
        }
    }
}
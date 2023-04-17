#define HIDE_ID

using System;
using System.ComponentModel;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmTrackList : BaseFrmTrackList
    {
        public FrmTrackList(Form owner) : base(owner)
        {
            InitializeComponent();
        }

        public override DataGridView DataGridView => dataGridView;
        public override DataGridViewColumn ColumnFind => ColumnId;

        private void FrmTrackList_Load(object sender, EventArgs e)
        {
#if !DEBUG || HIDE_ID
            ColumnId.Visible = false;
#endif
            dataGridView.Sort(ColumnDateTime, ListSortDirection.Ascending);

            ColumnDateTime.DefaultCellStyle.Format = Settings.Default.FormatDateTime;
            ColumnDistance.DefaultCellStyle.Format = Settings.Default.FormatDistance;
        }

        public override void Set(int rowIndex, TrackModel track)
        {
            dataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = track.Id;

            dataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = track.Text;

            dataGridView.Rows[rowIndex].Cells[ColumnDateTime.Name].Value = track.DateTime;

            dataGridView.Rows[rowIndex].Cells[ColumnDistance.Name].Value = track.Distance / 1000.0;
        }
    }
}
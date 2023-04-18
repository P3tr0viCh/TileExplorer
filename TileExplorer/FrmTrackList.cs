using System.ComponentModel;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class FrmTrackList : FrmListBase<TrackModel>
    {
        public override FrmListType Type => FrmListType.Tracks;

        private readonly DataGridViewTextBoxColumn ColumnText = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDateTime = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDistance = new DataGridViewTextBoxColumn();

        public FrmTrackList(Form owner) : base(owner)
        {
            Name = "FrmTrackList";
        }

        public override void Set(int rowIndex, TrackModel model)
        {
            DataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = model.Id;

            DataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = model.Text;

            DataGridView.Rows[rowIndex].Cells[ColumnDateTime.Name].Value = model.DateTime;

            DataGridView.Rows[rowIndex].Cells[ColumnDistance.Name].Value = model.Distance / 1000.0;
        }

        public override void InitializeComponent()
        {
            Text = "Треки";

            ColumnText.HeaderText = "Название";
            ColumnText.Name = "ColumnText";
            ColumnText.ReadOnly = true;
            ColumnText.Width = 144;

            ColumnDateTime.DefaultCellStyle = new DataGridViewCellStyle()
            {
                NullValue = null,
                Format = Settings.Default.FormatDateTime
            };
            ColumnDateTime.HeaderText = "Дата и время";
            ColumnDateTime.Name = "ColumnDateTime";
            ColumnDateTime.ReadOnly = true;
            ColumnDateTime.Width = 144;

            ColumnDistance.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                NullValue = null,
                Format = Settings.Default.FormatDistance
            };
            ColumnDistance.HeaderText = "Расстояние";
            ColumnDistance.Name = "ColumnDistance";
            ColumnDistance.ReadOnly = true;

            DataGridView.Columns.AddRange(new DataGridViewColumn[] { ColumnText, ColumnDateTime, ColumnDistance });

            DataGridView.Sort(ColumnDateTime, ListSortDirection.Ascending);
        }
    }
}
using System.ComponentModel;
using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class FrmMarkerList : FrmListBase<Models.Marker>
    {
        public override FrmListType Type => FrmListType.Markers;

        private readonly DataGridViewTextBoxColumn ColumnText = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnLat = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnLng = new DataGridViewTextBoxColumn();

        public FrmMarkerList(Form owner) : base(owner)
        {
            Name = "FrmMarkerList";
        }

        public override void Set(int rowIndex, Models.Marker model)
        {
            DataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = model.Id;

            DataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = model.Text;

            DataGridView.Rows[rowIndex].Cells[ColumnLat.Name].Value = model.Lat;
            DataGridView.Rows[rowIndex].Cells[ColumnLng.Name].Value = model.Lng;
        }

        public override void InitializeComponent()
        {
            Text = "Маркеры";

            ColumnText.HeaderText = "Текст";
            ColumnText.Name = "ColumnText";
            ColumnText.ReadOnly = true;
            ColumnText.Width = 144;

            ColumnLat.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                Format = Properties.Settings.Default.FormatLatLng
            };
            ColumnLat.HeaderText = "Широта";
            ColumnLat.Name = "ColumnLat";
            ColumnLat.ReadOnly = true;

            ColumnLng.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                Format = Properties.Settings.Default.FormatLatLng
            };
            ColumnLng.HeaderText = "Долгота";
            ColumnLng.Name = "ColumnLng";
            ColumnLng.ReadOnly = true;

            DataGridView.Columns.AddRange(new DataGridViewColumn[] { ColumnText, ColumnLat, ColumnLng });

            DataGridView.Sort(ColumnText, ListSortDirection.Ascending);
        }
    }
}
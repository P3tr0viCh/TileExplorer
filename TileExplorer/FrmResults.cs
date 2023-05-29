using System.ComponentModel;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class FrmResults : FrmListBase<Models.Results>
    {
        public override FrmListType Type => FrmListType.Results;

        private readonly DataGridViewTextBoxColumn ColumnYear = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnCount = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDistanceSum = new DataGridViewTextBoxColumn();

        public FrmResults(Form owner) : base(owner)
        {
            Name = "FrmResults";

            ColumnId.Visible = false;
        }

        public override void Set(int rowIndex, Models.Results model)
        {
            DataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = model.Id;

            if (model.Year != 0)
            {
                DataGridView.Rows[rowIndex].Cells[ColumnYear.Name].Value = model.Year;
            }
            else
            {
                DataGridView.Rows[rowIndex].Cells[ColumnYear.Name].Value = "";
            }

            DataGridView.Rows[rowIndex].Cells[ColumnCount.Name].Value = model.Count;

            DataGridView.Rows[rowIndex].Cells[ColumnDistanceSum.Name].Value = model.DistanceSum / 1000.0;
        }

        public override void InitializeComponent()
        {
            Text = "Итоги";

            ColumnYear.HeaderText = "Год";
            ColumnYear.Name = "ColumnYear";
            ColumnYear.ReadOnly = true;
            ColumnYear.Width = 64;

            ColumnCount.HeaderText = "Треки";
            ColumnCount.Name = "ColumnCount";
            ColumnCount.ReadOnly = true;
            ColumnCount.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight
            };

            ColumnDistanceSum.HeaderText = "Расстояние";
            ColumnDistanceSum.Name = "ColumnDistanceSum";
            ColumnDistanceSum.ReadOnly = true;
            ColumnDistanceSum.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                Format = AppSettings.Default.FormatDistance2
            };

            DataGridView.Columns.AddRange(new DataGridViewColumn[] { ColumnYear, ColumnCount, ColumnDistanceSum });

            DataGridView.SortCompare += new DataGridViewSortCompareEventHandler(DataGridView_SortCompare);

            DataGridView.Sort(ColumnYear, ListSortDirection.Ascending);
        }

        private void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((long)DataGridView.Rows[e.RowIndex1].Cells[ColumnId.Name].Value == 0)
            {
                e.SortResult = DataGridView.SortOrder == SortOrder.Ascending ? 1 : -1;
                e.Handled = true;
                return;
            }

            if ((long)DataGridView.Rows[e.RowIndex2].Cells[ColumnId.Name].Value == 0)
            {
                e.SortResult = DataGridView.SortOrder == SortOrder.Ascending ? -1 : 1;
                e.Handled = true;
                return;
            }

            e.Handled = false;
        }
    }
}
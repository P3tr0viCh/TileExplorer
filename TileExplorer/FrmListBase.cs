using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public interface IFrmChild
    {
        FrmListType Type { get; }
        bool Updating { set; }
    }

    public enum FrmListType
    {
        Markers,
        Tracks,
        Filter,
        Results
    }

    [DesignerCategory("")]
    public abstract class FrmListBase<T> : FrmList, IFrmChild where T : Models.BaseId
    {
        public abstract FrmListType Type { get; }

        protected FrmListBase(Form owner)
        {
            Owner = owner;

            ((ISupportInitialize)DataGridView).BeginInit();
            SuspendLayout();

#if !DEBUG
            ColumnId.Visible = false;
#endif

            InitializeComponent();

            ((ISupportInitialize)DataGridView).EndInit();
            ResumeLayout(false);

            DataGridView.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);
            DataGridView.MouseDoubleClick += new MouseEventHandler(DataGridView_MouseDoubleClick);
        }

        public abstract void InitializeComponent();

        public T Selected
        {
            set
            {
                if (!Visible) return;

                int rowIndex = Find(value);

                if (rowIndex == -1) return;

                if (rowIndex == DataGridView.CurrentCell.RowIndex) return;

                DataGridView.CurrentCell = DataGridView[
                    DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index, rowIndex];
            }
        }

        public List<T> List
        {
            set
            {
                DataGridView.Rows.Clear();

                foreach (var t in value)
                {
                    Add(t);
                }
            }
        }

        public int Find(T value)
        {
            if (value == null) return -1;

            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                if ((long)row.Cells[ColumnId.Name].Value == value.Id)
                {
                    return row.Index;
                }
            }

            return -1;
        }

        public void Add(T model)
        {
            Set(DataGridView.Rows.Add(), model);
        }

        public void Update(T model)
        {
            Set(Find(model), model);

            Sort();
        }

        public void Delete(T model)
        {
            int rowIndex = Find(model);

            if (rowIndex == -1) return;

            DataGridView.Rows.RemoveAt(rowIndex);
        }

        public abstract void Set(int rowIndex, T model);

        public void Sort()
        {
            DataGridView.Sort(DataGridView.SortedColumn,
                DataGridView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
        }

        private long GetSelectedId()
        {
            if (DataGridView.SelectedCells.Count == 0) return -1;

            var cellValue = DataGridView[ColumnId.Index, DataGridView.SelectedCells[0].RowIndex].Value;

            if (cellValue == null) return -1;

            return (long)cellValue;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var id = GetSelectedId();

            if (id == -1) return;

            (Owner as IMainForm).SelectMapItemById(this, id);
        }

        private void DataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var id = GetSelectedId();

            if (id == -1) return;

            (Owner as IMainForm).ChangeMapItemById(this, id);
        }
    }
}
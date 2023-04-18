using System;
using System.Drawing;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class FrmList : Form
    {
        public DataGridView DataGridView => dataGridView;
        public DataGridViewColumn ColumnId => colId;

        public FrmList()
        {
            InitializeComponent();

            SuspendLayout();

            lblUpdating.Text = Resources.ProgramStatusUpdating;

            pnlUpdating.Size = new Size(lblUpdating.Width + 32, lblUpdating.Height + 32);

            ResumeLayout(false);
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FrmList_SizeChanged(object sender, EventArgs e)
        {
            pnlUpdating.SetBounds((ClientSize.Width - pnlUpdating.Width) / 2, (ClientSize.Height - pnlUpdating.Height) / 2,
                pnlUpdating.Width, pnlUpdating.Height);
        }

        public bool Updating
        {
            set
            {
                pnlUpdating.Visible = value;
                UseWaitCursor = value;
            }
        }
    }
}
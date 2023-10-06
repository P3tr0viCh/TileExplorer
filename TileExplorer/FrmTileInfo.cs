using System.Drawing;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class FrmTileInfo : Form
    {
        public FrmTileInfo()
        {
            InitializeComponent();
        }

        public static FrmTileInfo ShowFrm(Form owner, Tile tile)
        {
            var frm = new FrmTileInfo()
            {
                Owner = owner,
                Text = string.Format(Resources.StatusTileId, tile.X, tile.Y),
            };

            frm.Location = new Point(owner.Location.X + owner.Width / 2 - frm.Width / 2,
                                     owner.Location.Y + owner.Height / 2 - frm.Height / 2);

            frm.LoadTileInfo(tile);

            frm.Show(owner);

            return frm;
        }

        private async void LoadTileInfo(Tile tile)
        {
            await Database.Default.LoadTileInfoAsync(tile);

            label1.Text = tile.Tracks.Count.ToString();
        }
    }
}
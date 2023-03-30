using System.Windows.Forms;

namespace TileExplorer
{
    public partial class FrmMarker : Form
    {
        public FrmMarker()
        {
            InitializeComponent();
        }

        public string MarkerText
        {
            get
            {
                return tbText.Text;
            }
            set
            {
                tbText.Text = value;
            }
        }

        public double MarkerLat
        {
            get
            {
                return (double)udPointLat.Value;
            }
            set
            {
                udPointLat.Value = (decimal)value;
            }
        }

        public double MarkerLng
        {
            get
            {
                return (double)udPointLng.Value;
            }
            set
            {
                udPointLng.Value = (decimal)value;
            }
        }

        public int OffsetX
        {
            get
            {
                return (int)udOffsetX.Value;
            }
            set
            {
                udOffsetX.Value = value;
            }
        }

        public int OffsetY
        {
            get
            {
                return (int)udOffsetY.Value;
            }
            set
            {
                udOffsetY.Value = value;
            }
        }
    }
}

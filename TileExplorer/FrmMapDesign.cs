using P3tr0viCh;
using System;
using System.Drawing;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class FrmMapDesign : Form
    {
        public FrmMapDesign()
        {
            InitializeComponent();
        }

        private Font FontMarker;
        private Color ColorMarkerText;

        public static bool ShowDlg(IWin32Window owner)
        {
            bool Result;

            using (var frm = new FrmMapDesign())
            {
                frm.FontMarker = Settings.Default.FontMarker;
                frm.ColorMarkerText = Settings.Default.ColorMarkerText;

                frm.tbFontMarker.Text = new FontConverter().ConvertToString(frm.FontMarker);
                frm.tbColorMarkerText.Text = new ColorConverter().ConvertToString(frm.ColorMarkerText);

                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    Settings.Default.FontMarker = frm.FontMarker;
                    Settings.Default.ColorMarkerText = frm.ColorMarkerText;

                    Settings.Default.Save();
                }
            }

            return Result;
        }

        private void BtnFontMarker_Click(object sender, System.EventArgs e)
        {
            fontDialog.Font = FontMarker;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                FontMarker = fontDialog.Font;

                tbFontMarker.Text = new FontConverter().ConvertToString(FontMarker);
            }
        }

        private Color StrToColor(string str)
        {
            try
            {
                return (Color)new ColorConverter().ConvertFromString(str);
            }
            catch (System.Exception)
            {
                return Color.Empty;
            }
        }

        private bool EditToColor(TextBox textBox, ref Color color)
        {
            Color clr = StrToColor(textBox.Text);

            if (clr != Color.Empty)
            {
                color = clr;
                return true;
            }

            textBox.Focus();

            Msg.Error();

            return false;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (EditToColor(tbColorMarkerText, ref ColorMarkerText))
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BtnFontColorMarker_Click(object sender, EventArgs e)
        {
            colorDialog.Color = ColorMarkerText;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ColorMarkerText = colorDialog.Color;

                tbColorMarkerText.Text = new ColorConverter().ConvertToString(ColorMarkerText);
            }
        }
    }
}
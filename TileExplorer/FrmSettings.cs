using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner)
        {
            using (var frm = new FrmSettings())
            {
                AppSettings.Default.Save();

                AppSettings.LoadFormState(frm, AppSettings.Default.FormStateSettings);

                frm.propertyGrid.SelectedObject = AppSettings.Default;

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(frm);

                    AppSettings.Default.Save();
                }
                else
                {
                    AppSettings.Default.Load();

                    AppSettings.Default.FormStateSettings = AppSettings.SaveFormState(frm);
                }


                return result;
            }
        }

        private bool CheckDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return true;

            if (Directory.Exists(path)) return true;

            Msg.Error(string.Format(Resources.ErrorDirectoryNotExists, path));

            return false;
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.PropertyType == AppSettings.Default.DatabaseHome.GetType())
            {
                CheckDirectory((string)e.ChangedItem.Value);
                return;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckDirectory(AppSettings.Default.DatabaseHome))
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
using System.Windows.Forms;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    internal class PresenterChildForm
    {
        private readonly Form Form;

        public PresenterChildForm(Form form)
        {
            Form = form;

            form.KeyPreview = true;

            form.KeyDown += Form_KeyDown;

            form.Load += Form_Load;

            form.FormClosed += Form_FormClosed;
        }

        private void Form_Load(object sender, System.EventArgs e)
        {
            (Form.Owner as IMainForm).ChildFormOpened(Form);
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            (Form.Owner as IMainForm).ChildFormClosed(Form);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Form.Close();
            }
        }
    }
}
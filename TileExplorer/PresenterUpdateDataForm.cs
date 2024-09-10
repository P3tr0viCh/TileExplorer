using System.Threading;
using System.Windows.Forms;

namespace TileExplorer
{
    internal class PresenterUpdateDataForm
    {
        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public CancellationToken CancelToken => cancelTokenSource.Token;

        public PresenterUpdateDataForm(Form form)
        {
            form.FormClosed += Form_FormClosed;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            cancelTokenSource.Cancel();
        }
    }
}
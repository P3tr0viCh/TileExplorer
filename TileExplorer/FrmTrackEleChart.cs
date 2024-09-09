using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTrackEleChart : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType => ChildFormType.TrackEleChart;

        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private Track Track { get; set; } = null;

        public FrmTrackEleChart()
        {
            InitializeComponent();
        }

        public static FrmTrackEleChart ShowFrm(Form owner, Track track)
        {
            Debug.Assert(owner is IMainForm);

            var frm = new FrmTrackEleChart()
            {
                Owner = owner,
                Text = track.Text,
                Track = track
            };

            frm.Location = new Point(owner.Location.X + owner.Width / 2 - frm.Width / 2,
                                     owner.Location.Y + owner.Height / 2 - frm.Height / 2);

            frm.Show(owner);

            return frm;
        }

        private void FrmTrackEleChart_Load(object sender, System.EventArgs e)
        {
            UpdateSettings();

            UpdateData();
        }

        public void UpdateSettings()
        {

        }

        public async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                chart.Series[0].Points.Clear();

                //await Task.Delay(3000, cancelTokenSource.Token);

                if (Track.TrackPoints == null)
                {
                    Track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(new { Track, full = true });
                }

                var min = double.MaxValue;
                var max = double.MinValue;

                var distance = 0.0;

                foreach (var point in Track.TrackPoints)
                {
                    if (point.Ele < min)
                    {
                        min = point.Ele;
                    }
                    if (point.Ele > max) { 
                        max = point.Ele; 
                    }

                    distance += point.Distance / 1000;

                    chart.Series[0].Points.AddXY(distance, point.Ele);
                }

                chart.ChartAreas[0].AxisY.Minimum = Math.Floor(min / 50.0) * 50.0; 
                chart.ChartAreas[0].AxisY.Maximum = Math.Floor(max / 50.0) * 50.0 + 50.0;

                chart.ChartAreas[0].AxisX.Minimum = 0;
                chart.ChartAreas[0].AxisX.Maximum = Track.Distance / 1000;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadTrackEleChartFail, e.Message);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
            }
        }

        public void UpdateData()
        {
            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () => await UpdateDataAsync());
            }, cancelTokenSource.Token);
        }

        private void FrmTrackEleChart_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }
    }
}
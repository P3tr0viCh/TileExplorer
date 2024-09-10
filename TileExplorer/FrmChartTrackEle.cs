using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmChartTrackEle : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType => ChildFormType.ChartTrackEle;

        internal readonly PresenterChildForm childFormPresenter;
        internal readonly PresenterUpdateDataForm updateDataFormPresenter;

        private Track Track { get; set; } = null;

        private ChartArea ChartArea => chart.ChartAreas[0];
        private Series ChartSerial => chart.Series[0];

        public FrmChartTrackEle()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
            updateDataFormPresenter = new PresenterUpdateDataForm(this);
        }

        public static FrmChartTrackEle ShowFrm(Form owner, Track track)
        {
            Debug.Assert(owner is IMainForm);

            var frm = new FrmChartTrackEle()
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

        private void FrmTrackEleChart_Load(object sender, EventArgs e)
        {
            UpdateSettings();

            UpdateData();
        }

        public void UpdateSettings()
        {
            ChartArea.AxisX.LineColor = AppSettings.Roaming.Default.ColorChartTrackEleAxis;
            ChartArea.AxisY.LineColor = AppSettings.Roaming.Default.ColorChartTrackEleAxis;

            ChartArea.AxisX.MajorGrid.LineColor = AppSettings.Roaming.Default.ColorChartTrackEleGrid;
            ChartArea.AxisY.MajorGrid.LineColor = AppSettings.Roaming.Default.ColorChartTrackEleGrid;

            ChartArea.AxisX.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartTrackEleText;
            ChartArea.AxisY.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartTrackEleText;

            ChartArea.CursorX.LineColor = AppSettings.Roaming.Default.ColorChartTrackEleCursor;

            ChartSerial.Color = Color.FromArgb(AppSettings.Roaming.Default.ColorChartTrackEleSerialAlpha,
                AppSettings.Roaming.Default.ColorChartTrackEleSerial);
        }

        public async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                ChartSerial.Points.Clear();

                ChartArea.CursorX.Position = double.NaN;

                //await Task.Delay(3000, cancelTokenSource.Token);

                if (Track.TrackPoints == null)
                {
                    Track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(new { Track, full = true });
                }

                var minEle = double.MaxValue;
                var maxEle = double.MinValue;

                var maxEleDistance = double.MinValue;

                var distance = 0.0;

                foreach (var point in Track.TrackPoints)
                {
                    distance += point.Distance / 1000;

                    if (point.Ele < minEle)
                    {
                        minEle = point.Ele;
                    }
                    if (point.Ele > maxEle)
                    {
                        maxEle = point.Ele;
                        maxEleDistance = distance;
                    }

                    ChartSerial.Points.AddXY(distance, point.Ele);
                }

                ChartArea.AxisY.Minimum = Math.Floor(minEle / 50.0) * 50.0;
                ChartArea.AxisY.Maximum = Math.Floor(maxEle / 50.0) * 50.0 + 50.0;

                ChartArea.AxisX.Minimum = 0;
                ChartArea.AxisX.Maximum = Track.Distance / 1000;
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
                CursorXPositionChanged();

                MainForm.ProgramStatus.Stop(status);
            }
        }

        public void UpdateData()
        {
            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () => await UpdateDataAsync());
            }, updateDataFormPresenter.CancelToken);
        }

        private bool AllowCursorXMove = true;

        private void CursorXPositionChanged()
        {
            var dist = ChartArea.CursorX.Position;

            if (dist is double.NaN)
            {
                slDist.Text = string.Empty;

                slEle.Text = string.Empty;
            }
            else
            {
                slDist.Text = string.Format(Resources.StatusDist, dist);

                if (dist == 0)
                {
                    slEle.Text = string.Format(Resources.StatusEle, ChartSerial.Points.First().YValues.First());

                    return;
                }

                var pointPrev = ChartSerial.Points.Select(x => x)
                                        .Where(x => x.XValue < dist)
                                        .DefaultIfEmpty(ChartSerial.Points.First()).Last();
                var pointNext = ChartSerial.Points.Select(x => x)
                                        .Where(x => x.XValue >= dist)
                                        .DefaultIfEmpty(ChartSerial.Points.Last()).First();

                var x1 = pointPrev.XValue;
                var y1 = pointPrev.YValues.First();

                var x2 = pointNext.XValue;
                var y2 = pointNext.YValues.First();

                var ele = Utils.LinearInterpolate(dist, x1, y1, x2, y2);

                slEle.Text = string.Format(Resources.StatusEle, ele);
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (AllowCursorXMove)
            {
                ChartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);

                CursorXPositionChanged();
            }
        }

        private void Chart_MouseClick(object sender, MouseEventArgs e)
        {
            AllowCursorXMove = !AllowCursorXMove;

            CursorXPositionChanged();
        }
    }
}
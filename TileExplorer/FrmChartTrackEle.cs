using GMap.NET;
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

        public ChildFormType FormType => ChildFormType.ChartTrackEle;

        internal readonly PresenterChildForm childFormPresenter;

        public Track Track { get; private set; } = null;

        private ChartArea ChartArea => chart.ChartAreas[0];
        private Series ChartSerial => chart.Series[0];

        public FrmChartTrackEle()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
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
            CursorXPositionChanged();

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

        private CancellationTokenSource cancellationTokenSource;

        public async Task UpdateDataAsync()
        {
            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                ChartSerial.Points.Clear();

                ChartArea.CursorX.Position = double.NaN;

                //await Task.Delay(3000, cancellationTokenSource.Token);

                Track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(new { track = Track, full = true });

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
            cancellationTokenSource?.Cancel();

            cancellationTokenSource?.Dispose();

            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                this.InvokeIfNeeded(async () => await UpdateDataAsync());
            }, cancellationTokenSource.Token);
        }

        private bool AllowCursorXMove = true;

        private PointLatLng CursorPoint = default;

        private void CursorXPositionChanged()
        {
            var dist = ChartArea.CursorX.Position;

            if (dist is double.NaN || ChartSerial.Points.Count == 0)
            {
                slDist.Text = string.Empty;

                slEle.Text = string.Empty;

                MainForm.ShowMarkerPosition(this, default);

                return;
            }

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

            dist *= 1000;

            var distance = 0D;

            CursorPoint = default;

            foreach (var point in Track.TrackPoints)
            {
                distance += point.Distance;

                if (distance >= dist)
                {
                    CursorPoint = new PointLatLng(point.Lat, point.Lng);

                    break;
                }
            }

            MainForm?.ShowMarkerPosition(this, CursorPoint);
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

        private void FrmChartTrackEle_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm?.ShowMarkerPosition(this, default);

            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            MainForm?.ShowMarkerPosition(this, default);
        }

        private void Chart_MouseEnter(object sender, EventArgs e)
        {
            MainForm?.SelectMapItem(this, Track);

            MainForm?.ShowMarkerPosition(this, CursorPoint);
        }
    }
}
using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmChartTrackEle : Form, IChildForm, IUpdateDataForm, PresenterStatusStripChartTrackEle.IStatusStripView
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.ChartTrackEle;

        internal readonly PresenterChildForm childFormPresenter;

        private readonly PresenterStatusStripChartTrackEle statusStripPresenter;

        public Track Track { get; private set; } = null;

        private ChartArea ChartArea => chart.ChartAreas[0];
        private Series ChartSerial => chart.Series[0];

        public FrmChartTrackEle()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);

            statusStripPresenter = new PresenterStatusStripChartTrackEle(this);
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

        private async void FrmTrackEleChart_Load(object sender, EventArgs e)
        {
            ChartArea.AxisX.ScaleView.Zoomable = false;

            CursorXPositionChanged();

            UpdateSettings();

            await UpdateDataAsync();
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

        ToolStripStatusLabel PresenterStatusStripChartTrackEle.IStatusStripView.GetLabel(PresenterStatusStripChartTrackEle.StatusLabel label)
        {
            switch (label)
            {
                case PresenterStatusStripChartTrackEle.StatusLabel.Ele: return slEle;
                case PresenterStatusStripChartTrackEle.StatusLabel.Distance: return slDistance;
                case PresenterStatusStripChartTrackEle.StatusLabel.DateTime: return slDateTime;
                case PresenterStatusStripChartTrackEle.StatusLabel.DateTimeSpan: return slDateTimeSpan;
                case PresenterStatusStripChartTrackEle.StatusLabel.IsSelection: return slIsSelection;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private readonly WrapperCancellationTokenSource ctsChartTrackEle = new WrapperCancellationTokenSource();

        public async Task UpdateDataAsync()
        {
            ctsChartTrackEle.Start();

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                ChartSerial.Points.Clear();

                ChartArea.CursorX.Position = double.NaN;

                // await Task.Delay(3000, ctsChartTrackEle.Token);

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

                ChartArea.CursorX.Position = 0;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadChartTrackEleFail, e.Message);
            }
            finally
            {
                ctsChartTrackEle.Finally();

                CursorXPositionChanged();

                MainForm.ProgramStatus.Stop(status);
            }
        }

        private bool AllowCursorXMove = true;

        private PointLatLng CursorPoint = default;

        private void CursorXPositionChanged()
        {
            var distanceFromStart = ChartArea.CursorX.Position;

            if (distanceFromStart is double.NaN || ChartSerial.Points.Count == 0)
            {
                statusStripPresenter.Ele = 0;
                statusStripPresenter.Distance = 0;
                statusStripPresenter.DateTime = default;
                statusStripPresenter.DateTimeSpan = default;

                MainForm.ShowMarkerPosition(this, default);

                return;
            }

            CursorPoint = default;

            var point = default(TrackPoint);

            if (distanceFromStart == 0)
            {
                point = Track.TrackPoints.First();

                statusStripPresenter.Ele = point.Ele;
                statusStripPresenter.Distance = 0;
                statusStripPresenter.DateTime = point.DateTime;
                statusStripPresenter.DateTimeSpan = default;

                CursorPoint = new PointLatLng(point.Lat, point.Lng);

                MainForm?.ShowMarkerPosition(this, CursorPoint);

                return;
            }

            if (Utils.DoubleEquals(distanceFromStart * 1000, Track.Distance, 0.0001))
            {
                point = Track.TrackPoints.Last();

                statusStripPresenter.Ele = point.Ele;
                statusStripPresenter.Distance = Track.Distance;
                statusStripPresenter.DateTime = point.DateTime;
                statusStripPresenter.DateTimeSpan = Track.DateTimeFinish - Track.DateTimeStart;

                CursorPoint = new PointLatLng(point.Lat, point.Lng);

                MainForm?.ShowMarkerPosition(this, CursorPoint);

                return;
            }

            var pointPrev = ChartSerial.Points.Select(x => x)
                                    .Where(x => x.XValue < distanceFromStart)
                                    .DefaultIfEmpty(ChartSerial.Points.First()).Last();
            var pointNext = ChartSerial.Points.Select(x => x)
                                    .Where(x => x.XValue >= distanceFromStart)
                                    .DefaultIfEmpty(ChartSerial.Points.Last()).First();

            var dist1 = pointPrev.XValue;
            var ele1 = pointPrev.YValues.First();

            var dist2 = pointNext.XValue;
            var ele2 = pointNext.YValues.First();

            var ele = Utils.LinearInterpolate(distanceFromStart, dist1, ele1, dist2, ele2);

            var distance = 0D;
            var dateTime = default(DateTime);
            var dateTimeSpan = default(TimeSpan);

            CursorPoint = default;

            distanceFromStart *= 1000;

            foreach (var trackPoint in Track.TrackPoints)
            {
                distance += trackPoint.Distance;

                if (distance >= distanceFromStart)
                {
                    CursorPoint = new PointLatLng(trackPoint.Lat, trackPoint.Lng);

                    dateTime = trackPoint.DateTime;

                    dateTimeSpan = dateTime - Track.DateTimeStart;

                    break;
                }
            }

            statusStripPresenter.Ele = ele;
            statusStripPresenter.Distance = distance;
            statusStripPresenter.DateTime = dateTime;
            statusStripPresenter.DateTimeSpan = dateTimeSpan;

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
            if (ChartArea.CursorX.SelectionStart != ChartArea.CursorX.SelectionEnd)
            {
                AllowCursorXMove = true;
                return;
            }

            AllowCursorXMove = !AllowCursorXMove;

            CursorXPositionChanged();
        }

        private void FrmChartTrackEle_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm?.ShowMarkerPosition(this, default);

            ctsChartTrackEle.Cancel();
        }

        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            MainForm?.ShowMarkerPosition(this, default);
        }

        private void Chart_MouseEnter(object sender, EventArgs e)
        {
            MainForm?.SelectMapItemAsync(this, Track);

            MainForm?.ShowMarkerPosition(this, CursorPoint);
        }

        private void Chart_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            var isSelection = e.NewSelectionStart != e.NewSelectionEnd;

            statusStripPresenter.IsSelection = isSelection;

            if (!isSelection)
            {
                return;
            }

            //TODO
        }
    }
}
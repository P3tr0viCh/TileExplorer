using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using  TileExplorer.Interfaces;
using TileExplorer.Presenters;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Presenters.PresenterStatusStripChartTrackEle;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class FrmChartTrackEle : Form, IChildForm, IUpdateDataForm, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.ChartTrackEle;

        private readonly PresenterStatusStripChartTrackEle statusStripPresenter;

        public Track Track { get; private set; } = null;

        private ChartArea ChartArea => chart.ChartAreas[0];
        private Series ChartSerial => chart.Series[0];

        public FrmChartTrackEle()
        {
            InitializeComponent();

            PresenterChildForm.LinkTo(this);

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

            ChartArea.AxisX.LabelStyle.Format = Resources.TextChartTrackEleAxisX;
            ChartArea.AxisY.LabelStyle.Format = Resources.TextChartTrackEleAxisY;

            CursorXPositionChanged();

            UpdateSettings();

            await UpdateDataAsync();
        }

        public void UpdateSettings()
        {
            ChartArea.AxisX.LineColor = AppSettings.Roaming.Default.ColorChartAxis;
            ChartArea.AxisY.LineColor = AppSettings.Roaming.Default.ColorChartAxis;

            ChartArea.AxisX.MajorGrid.LineColor = AppSettings.Roaming.Default.ColorChartGrid;
            ChartArea.AxisY.MajorGrid.LineColor = AppSettings.Roaming.Default.ColorChartGrid;

            ChartArea.AxisX.MajorTickMark.LineColor = AppSettings.Roaming.Default.ColorChartAxis;
            ChartArea.AxisY.MajorTickMark.LineColor = AppSettings.Roaming.Default.ColorChartAxis;

            ChartArea.AxisX.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartText;
            ChartArea.AxisY.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartText;

            ChartArea.CursorX.LineColor = AppSettings.Roaming.Default.ColorChartCursor;

            ChartSerial.Color = Color.FromArgb(AppSettings.Roaming.Default.ColorChartSerialAlpha,
                AppSettings.Roaming.Default.ColorChartSerial);
        }

        ToolStripStatusLabel PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip.GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Ele: return slEle;
                case StatusLabel.Distance: return slDistance;
                case StatusLabel.DateTime: return slDateTime;
                case StatusLabel.DateTimeSpan: return slDateTimeSpan;
                case StatusLabel.IsSelection: return slIsSelection;
                case StatusLabel.SelectedEleAscent: return slSelectedEleAscent;
                case StatusLabel.SelectedEleDescent: return slSelectedEleDescent;
                case StatusLabel.SelectedDistance: return slSelectedDistance;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private readonly WrapperCancellationTokenSource ctsChartTrackEle = new WrapperCancellationTokenSource();

        public async Task UpdateDataAsync()
        {
            ctsChartTrackEle.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                ChartSerial.Points.Clear();

                ChartArea.CursorX.Position = double.NaN;

                // await Task.Delay(3000, ctsChartTrackEle.Token);

                var trackPoints = await Database.Default.ListLoadAsync<TrackPoint>(new { track = Track, full = true });

                Track.TrackPoints = trackPoints.ToList();

                var minEle = double.MaxValue;
                var maxEle = double.MinValue;

                var maxEleDistance = double.MinValue;

                var distance = 0.0;

                foreach (var point in Track.TrackPoints)
                {
                    distance += point.Distance;

                    if (point.Ele < minEle)
                    {
                        minEle = point.Ele;
                    }
                    if (point.Ele > maxEle)
                    {
                        maxEle = point.Ele;
                        maxEleDistance = distance;
                    }

                    ChartSerial.Points.Add(new DataPoint(distance, point.Ele)
                    {
                        Tag = point
                    });
                }

                ChartArea.AxisY.Minimum = Utils.DoubleFloorToEpsilon(minEle, ChartArea.AxisY.Interval);
                ChartArea.AxisY.Maximum = Utils.DoubleFloorToEpsilon(maxEle, ChartArea.AxisY.Interval) + ChartArea.AxisY.Interval;

                ChartArea.AxisX.Minimum = 0;
                ChartArea.AxisX.Maximum = Track.Distance;

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

                ProgramStatus.Default.Stop(status);
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

            if (Utils.DoubleEquals(distanceFromStart, Track.Distance, 0.001))
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

            var trackPoint = (TrackPoint)pointPrev.Tag;

            CursorPoint = new PointLatLng(trackPoint.Lat, trackPoint.Lng);

            statusStripPresenter.Ele = ele;
            statusStripPresenter.Distance = distanceFromStart;
            statusStripPresenter.DateTime = trackPoint.DateTime;
            statusStripPresenter.DateTimeSpan = trackPoint.DateTime - Track.DateTimeStart;

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

            var selected = new Gpx.Track
            {
                Points = new List<Gpx.Point>()
            };

            var distanceSelectedStart = e.NewSelectionStart;
            var distanceSelectedEnd = e.NewSelectionEnd;

            if (distanceSelectedStart > distanceSelectedEnd)
            {
                (distanceSelectedStart, distanceSelectedEnd) = (distanceSelectedEnd, distanceSelectedStart);
            }

            DebugWrite.Line($"distanceSelectedStart = {distanceSelectedStart}, " +
                            $"distanceSelectedEnd = {distanceSelectedEnd}");

            var pointStart = (TrackPoint)ChartSerial.Points.Select(x => x)
                                    .Where(x => x.XValue < distanceSelectedStart)
                                    .DefaultIfEmpty(ChartSerial.Points.First()).Last().Tag;
            var pointEnd = (TrackPoint)ChartSerial.Points.Select(x => x)
                                    .Where(x => x.XValue >= distanceSelectedEnd)
                                    .DefaultIfEmpty(ChartSerial.Points.Last()).First().Tag;

            DebugWrite.Line($"pointStart = {pointStart.DateTime}|{pointStart.Ele}, " +
                            $"pointEnd = {pointEnd.DateTime}|{pointEnd.Ele}");

            selected.Points.AddRange(Track.TrackPoints.Select(p => p)
                .Where(p => p.Num >= pointStart.Num && p.Num <= pointEnd.Num).ToList());

            DebugWrite.Line($"selected.first = {selected.Points.First().DateTime}|{selected.Points.First().Ele}, " +
                $"selected.last = {selected.Points.Last().DateTime}|{selected.Points.Last().Ele}");

            if (selected.Points.Count < 2)
            {
                statusStripPresenter.IsSelection = false;
                return;
            }

            selected.NotifyPointsChanged();

            statusStripPresenter.SelectedEleAscent = selected.EleAscent;
            statusStripPresenter.SelectedEleDescent = selected.EleDescent;
            statusStripPresenter.SelectedDistance = selected.Distance;
        }
    }
}
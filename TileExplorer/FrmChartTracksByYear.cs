using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TileExplorer.Properties;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmChartTracksByYear : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.ChartTracksByYear;

        internal readonly PresenterChildForm childFormPresenter;

        private readonly (int row, int col)[] grid = { (0, 0), (0, 1), (0, 2), (0, 3),
                                                       (1, 0), (1, 1), (1, 2), (1, 3),
                                                       (2, 0), (2, 1), (2, 2), (2, 3) };

        private Chart[] Charts { get; set; } = new Chart[12];
        private Title[] ChartTitleMonths { get; set; } = new Title[12];
        private Title[] ChartTitleCounts { get; set; } = new Title[12];
        private ChartArea[] ChartAreas { get; set; } = new ChartArea[12];
        private Series[] ChartSerial { get; set; } = new Series[12];

        public int Year { get; private set; }

        public FrmChartTracksByYear()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
        }

        public static FrmChartTracksByYear ShowFrm(Form owner, int year)
        {
            Debug.Assert(owner is IMainForm);

            var frm = new FrmChartTracksByYear
            {
                Owner = owner,
                Year = year,
            };

            frm.Location = new Point(owner.Location.X + owner.Width / 2 - frm.Width / 2,
                                     owner.Location.Y + owner.Height / 2 - frm.Height / 2);

            frm.Show(owner);

            return frm;
        }

        private async void FrmChartTracksByYear_Load(object sender, EventArgs e)
        {
            Text = Resources.TitleTracksByYear;

            CreateCharts();

            UpdateSettings();

            selfChange = false;

            await UpdateDataAsync();
        }

        private void CreateCharts()
        {
            var month = 0;

            SuspendLayout();

            for (var i = 0; i < 12; i++)
            {
                Charts[i] = new Chart();
                ChartAreas[i] = new ChartArea();
                ChartTitleMonths[i] = new Title();
                ChartTitleCounts[i] = new Title();
                ChartSerial[i] = new Series();

                month = i + 1;

                ((ISupportInitialize)Charts[i]).BeginInit();

                ChartAreas[i].AxisX.ScaleView.Zoomable = false;
                ChartAreas[i].AxisX.LabelStyle.Font = new Font("Segoe UI", 10F);
                ChartAreas[i].AxisY.LabelStyle.Font = new Font("Segoe UI", 10F);
                ChartAreas[i].AxisX.Interval = 1D;
                ChartAreas[i].AxisY.Interval = 10;
                ChartAreas[i].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ChartAreas[i].AxisX.IntervalOffset = 1D;
                ChartAreas[i].AxisX.IntervalOffsetType = DateTimeIntervalType.Days;
                ChartAreas[i].AxisX.IntervalType = DateTimeIntervalType.Days;
                ChartAreas[i].AxisX.IsStartedFromZero = false;
                ChartAreas[i].AxisX.LabelStyle.Enabled = false;
                ChartAreas[i].AxisY.LabelStyle.Enabled = false;
                ChartAreas[i].AxisX.MajorGrid.Enabled = false;
                ChartAreas[i].AxisY.MajorGrid.Enabled = false;
                ChartAreas[i].AxisX.MajorTickMark.Enabled = false;
                ChartAreas[i].AxisY.MajorTickMark.Enabled = false;

                ChartTitleMonths[i].Text = Utils.GetMonthName(month);
                ChartTitleMonths[i].Font = new Font("Segoe UI", 10F);
                ChartTitleMonths[i].Alignment = ContentAlignment.TopLeft;
                ChartTitleMonths[i].Position.Auto = false;
                ChartTitleMonths[i].Position.X = 4;
                ChartTitleMonths[i].Position.Y = 4;

                ChartTitleCounts[i].Text = string.Empty;
                ChartTitleCounts[i].Font = new Font("Segoe UI", 10F);
                ChartTitleCounts[i].Alignment = ContentAlignment.TopRight;
                ChartTitleCounts[i].Position.Auto = false;
                ChartTitleCounts[i].Position.X = 96;
                ChartTitleCounts[i].Position.Y = 4;

                Charts[i].ChartAreas.Add(ChartAreas[i]);

                Charts[i].Titles.Add(ChartTitleMonths[i]);
                Charts[i].Titles.Add(ChartTitleCounts[i]);

                Charts[i].Series.Add(ChartSerial[i]);

                Charts[i].TabIndex = i + 1;

                Charts[i].ContextMenuStrip = contextMenuStrip;

                Charts[i].Tag = month;

                Charts[i].DoubleClick += FrmChartTracksByYear_ChartDoubleClick;

                Controls.Add(Charts[i]);

                ((ISupportInitialize)Charts[i]).EndInit();
            }

            ResumeLayout(false);
            PerformLayout();

            UpdateChartsBounds();
        }

        private int GetChartLineWidth(int chartWidth)
        {
            if (chartWidth <= 100) return 1;

            if (chartWidth <= 200) return 2;

            if (chartWidth <= 400) return 4;

            return 8;
        }

        private void UpdateChartsBounds()
        {
            SuspendLayout();

            var top = toolStrip.Height;

            var width = ClientSize.Width / 4;
            var height = (ClientSize.Height - top) / 3;

            for (var i = 0; i < 12; i++)
            {
                Charts[i].Bounds = new Rectangle(grid[i].col * width, top + grid[i].row * height, width, height);

                ChartSerial[i]["PixelPointWidth"] = $"{GetChartLineWidth(Charts[i].Bounds.Width)}";
            }

            ResumeLayout(false);
        }

        public void UpdateSettings()
        {
            for (var i = 0; i < 12; i++)
            {
                ChartAreas[i].AxisX.LineColor = AppSettings.Roaming.Default.ColorChartAxis;
                ChartAreas[i].AxisY.LineColor = AppSettings.Roaming.Default.ColorChartAxis;

                ChartAreas[i].AxisX.MajorTickMark.LineColor = AppSettings.Roaming.Default.ColorChartAxis;
                ChartAreas[i].AxisY.MajorTickMark.LineColor = AppSettings.Roaming.Default.ColorChartAxis;

                ChartAreas[i].AxisX.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartText;
                ChartAreas[i].AxisY.LabelStyle.ForeColor = AppSettings.Roaming.Default.ColorChartText;

                ChartSerial[i].LabelForeColor = AppSettings.Roaming.Default.ColorChartText;

                ChartSerial[i].Color = Color.FromArgb(AppSettings.Roaming.Default.ColorChartSerialAlpha,
                    AppSettings.Roaming.Default.ColorChartSerial);
            }
        }

        private bool selfChange = true;

        private readonly WrapperCancellationTokenSource ctsChartTracksByYear = new WrapperCancellationTokenSource();

        public async Task UpdateDataAsync()
        {
            ctsChartTracksByYear.Start();

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                Debug.WriteLine($"year: {Year}");

                for (var i = 0; i < 12; i++)
                {
                    selfChange = true;

                    cboxYear.ComboBox.DataSource = MainForm.Years;

                    cboxYear.SelectedItem = Year;

                    selfChange = false;

                    ChartSerial[i].Points.Clear();

                    var month = i + 1;

                    var daysInMonth = DateTime.DaysInMonth(Year, month);

                    ChartAreas[i].AxisX.Minimum = new DateTime(Year, month, 1).AddDays(-1).ToOADate();
                    ChartAreas[i].AxisX.Maximum = new DateTime(Year, month, daysInMonth).AddDays(1).ToOADate();

                    DateTime date;
                    double dateOA;

                    for (var day = 1; day <= daysInMonth; day++)
                    {
                        date = new DateTime(Year, month, day);

                        dateOA = date.ToOADate();

                        ChartSerial[i].Points.Add(new DataPoint(dateOA, 0D)
                        {
                            IsEmpty = true,
                        });
                    }

                    var distances = await Database.Default.ListLoadAsync<Database.Models.TracksDistanceByMonth>(
                        new { year = Year, month });

                    DebugWrite.Line(string.Join(", ", distances.Select(d => d.Distance)));

                    var counts = string.Empty;

                    if (distances.Count > 0)
                    {
                        var maxDistance = 0D;

                        var allDistances = 0D;

                        foreach (var distance in distances)
                        {
                            if (distance.Distance > maxDistance)
                            {
                                maxDistance = distance.Distance;
                            }

                            ChartSerial[i].Points[distance.Day - 1].YValues[0] = distance.Distance / 1000;
                            ChartSerial[i].Points[distance.Day - 1].IsEmpty = false;

                            allDistances += distance.Distance;   
                        }

                        ChartAreas[i].AxisY.Maximum = Utils.DoubleFloorToEpsilon(maxDistance / 1000, ChartAreas[i].AxisY.Interval) + ChartAreas[i].AxisY.Interval;

                        allDistances /= 1000;

                        counts = string.Format(Resources.TextChartTracksByYearCounts, distances.Count, allDistances);
                    }
                    else
                    {
                        ChartAreas[i].AxisY.Maximum = ChartAreas[i].AxisY.Interval;
                    }

                    ChartTitleCounts[i].Text = counts;
                }
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadChartTracksByYearFail, e.Message);
            }
            finally
            {
                ctsChartTracksByYear.Finally();

                MainForm.ProgramStatus.Stop(status);
            }
        }

        private async void CboxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selfChange)
            {
                return;
            }

            Year = (int)cboxYear.SelectedItem;

            await UpdateDataAsync();
        }

        private void FrmChartTracksByYear_ClientSizeChanged(object sender, EventArgs e)
        {
            UpdateChartsBounds();
        }

        private void FrmChartTracksByYear_FormClosed(object sender, FormClosedEventArgs e)
        {
            ctsChartTracksByYear.Cancel();
        }

        private void FrmChartTracksByYear_ChartDoubleClick(object sender, EventArgs e)
        {
            Utils.Forms.OpenChartTracksByMonth((Form)MainForm, Year, (int)((Chart)sender).Tag);
        }

        private void MiOpenChartTracksByMonth_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var contextMenu = (ContextMenuStrip)menuItem.Owner;
            var chart = (Chart)contextMenu.SourceControl;

            Utils.Forms.OpenChartTracksByMonth((Form)MainForm, Year, (int)chart.Tag);
        }
    }
}
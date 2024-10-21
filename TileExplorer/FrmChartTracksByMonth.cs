using P3tr0viCh.Utils;
using System;
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
    public partial class FrmChartTracksByMonth : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.ChartTracksByMonth;

        internal readonly PresenterChildForm childFormPresenter;

        private ChartArea ChartArea => chart.ChartAreas[0];
        private Series ChartSerial => chart.Series[0];

        public int Year { get; private set; }
        public int Month { get; private set; }

        public FrmChartTracksByMonth()
        {
            InitializeComponent();

            childFormPresenter = new PresenterChildForm(this);
        }

        public static FrmChartTracksByMonth ShowFrm(Form owner, int year, int month)
        {
            Debug.Assert(owner is IMainForm);

            var frm = new FrmChartTracksByMonth
            {
                Owner = owner,
                Year = year,
                Month = month,
            };

            frm.Location = new Point(owner.Location.X + owner.Width / 2 - frm.Width / 2,
                                     owner.Location.Y + owner.Height / 2 - frm.Height / 2);

            frm.Show(owner);

            return frm;
        }

        private async void FrmChartTracksByMonth_Load(object sender, EventArgs e)
        {
            Text = Resources.TitleTracksByMonth;

            ChartArea.AxisX.ScaleView.Zoomable = false;

            ChartArea.AxisX.LabelStyle.Format = Resources.TextChartTracksByMonthAxisX;
            ChartArea.AxisY.LabelStyle.Format = Resources.TextChartTracksByMonthAxisY;

            ChartSerial.LabelFormat = Resources.TextChartTracksByMonthAxisY;

            ChartArea.AxisY.Interval = 10000;

            UpdateSettings();

            FillMonths();

            cboxMonth.SelectedIndex = Month - 1;

            selfChange = false;

            await UpdateDataAsync();
        }

        private void FillMonths()
        {
            for (var month = 1; month <= 12; month++)
            {
                cboxMonth.Items.Add(new DateTime(Year, month, 1).ToString("MMMM"));
            }
        }

        public void UpdateSettings()
        {
        }


        private bool selfChange = true;

        private readonly WrapperCancellationTokenSource ctsChartTracksByMonth = new WrapperCancellationTokenSource();

        public async Task UpdateDataAsync()
        {
            ctsChartTracksByMonth.Start();

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                Debug.WriteLine($"year: {Year}, month: {Month}");

                ChartSerial.Points.Clear();

                selfChange = true;

                cboxYear.ComboBox.DataSource = MainForm.Years;

                cboxYear.SelectedItem = Year;

                selfChange = false;

                var daysInMonth = DateTime.DaysInMonth(Year, Month);

                ChartArea.AxisX.Minimum = new DateTime(Year, Month, 1).AddDays(-1).ToOADate();
                ChartArea.AxisX.Maximum = new DateTime(Year, Month, daysInMonth).ToOADate();

                for (var day = 1; day <= daysInMonth; day++)
                {
                    ChartSerial.Points.Add(new DataPoint(new DateTime(Year, Month, day).ToOADate(), 0D)
                    {
                        IsEmpty = true
                    });
                }
                
                var distances = await Database.Default.ListLoadAsync<Database.Models.TracksDistanceByMonth>(
                    new { year = Year, month = Month });

                DebugWrite.Line(string.Join(", ", distances.Select(d => d.Distance)));

                if (distances.Count > 0)
                {
                    var maxDistance = 0D;

                    foreach (var distance in distances)
                    {
                        if (distance.Distance > maxDistance)
                        {
                            maxDistance = distance.Distance;
                        }

                        ChartSerial.Points[distance.Day - 1].YValues[0] = distance.Distance;
                        ChartSerial.Points[distance.Day - 1].IsEmpty = false;
                    }

                    ChartArea.AxisY.Maximum = Utils.DoubleFloorToEpsilon(maxDistance, ChartArea.AxisY.Interval) + ChartArea.AxisY.Interval;
                }
                else
                {
                    ChartArea.AxisY.Maximum = ChartArea.AxisY.Interval;
                }
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadChartTracksByMonthFail, e.Message);
            }
            finally
            {
                ctsChartTracksByMonth.Finally();

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

        private async void CboxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selfChange)
            {
                return;
            }

            Month = cboxMonth.SelectedIndex + 1;

            await UpdateDataAsync();
        }
    }
}
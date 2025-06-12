using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class HomeView : UserControl
    {
        public SeriesCollection StudentTrendSeries { get; set; }
        public string[] StudentTrendLabels { get; set; }
        public Func<double, string> TrendFormatter { get; set; }
        public List<string> TimeLabels { get; set; }
        private int _currentMonth;
        private DispatcherTimer _trendTimer;
        private Random _random = new Random();

        public HomeView()
        {
            InitializeComponent();

            DataContext = this;

            LoadReportData();
            InitializeTrendChart();
        }

        #region HOME VIEW RENDERING

        private async void LoadReportData()
        {
            var report = await FinanceReportService.GetFinanceSummaryAsync();

            if (report != null)
            {
                AnimateNumber(StudentCountText, report.StudentIncome?.TotalStudents ?? 0);
                AnimateNumber(EmployeeCountText, report.TotalEmployees);
                AnimateDecimal(NetProfitText, report.NetProfit);
            }
            else
            {
                StudentCountText.Text = "0";
                EmployeeCountText.Text = "0";
                NetProfitText.Text = "₴0";
            }
        }

        private void InitializeTrendChart()
        {
            var now = DateTime.Now;
            _currentMonth = now.Month;

            TimeLabels = new List<string>();
            StudentTrendSeries = new SeriesCollection();
            var random = new Random();

            var values = new ChartValues<int>();

            for (int month = 1; month <= now.Month; month++)
            {
                var date = new DateTime(now.Year, month, 1);

                TimeLabels.Add(date.ToString("MMMM", new System.Globalization.CultureInfo("en-EN")));
                values.Add(random.Next(5, 25));
            }

            StudentTrendSeries.Add(new LineSeries
            {
                Title = "Students",
                Values = values,
                PointGeometrySize = 8,
                StrokeThickness = 3,
                Fill = new LinearGradientBrush(
                    Colors.Transparent,
                    Color.FromArgb(50, 33, 150, 243),
                    90),
                LineSmoothness = 0.8
            });

            StudentTrendLabels = TimeLabels.ToArray();
            TrendFormatter = value => value.ToString("N0");

            _trendTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            _trendTimer.Tick += (s, e) =>
            {
                var lineSeries = StudentTrendSeries[0] as LineSeries;
                if (lineSeries == null) return;

                var valuesList = lineSeries.Values.Cast<int>().ToList();
                int lastIntValue = valuesList.Last();
                double lastValue = Convert.ToDouble(lastIntValue);

                double newValue = lastValue + _random.NextDouble() * 4 - 2;
                lineSeries.Values.Add((int)newValue);

                if (lineSeries.Values.Count > 12)
                {
                    lineSeries.Values.RemoveAt(0);
                    TimeLabels.RemoveAt(0);
                }

                _currentMonth++;
                if (_currentMonth > 12)
                    _currentMonth = 1;

                var newMonthName = new DateTime(now.Year, _currentMonth, 1)
                    .ToString("MMMM", new System.Globalization.CultureInfo("en-EN"));

                TimeLabels.Add(newMonthName);
                StudentTrendLabels = TimeLabels.ToArray();

                lineSeries.Stroke = new SolidColorBrush(newValue >= lastValue ? Colors.LimeGreen : Colors.Red);

                var chart = StudentTrendChart;
                if (chart != null)
                {
                    var scaleTransform = new ScaleTransform(1.0, 1.0);
                    chart.RenderTransformOrigin = new Point(0.5, 0.5);
                    chart.RenderTransform = scaleTransform;

                    var bounceAnimation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 1.05,
                        AutoReverse = true,
                        Duration = TimeSpan.FromMilliseconds(200),
                        EasingFunction = new BounceEase
                        {
                            Bounces = 1,
                            Bounciness = 1.5,
                            EasingMode = EasingMode.EaseOut
                        }
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, bounceAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bounceAnimation);
                }

                DataContext = null;
                DataContext = this;
            };

            _trendTimer.Start();
        }

        private async void AnimateNumber(TextBlock textBlock, int targetValue)
        {
            int durationMs = CalculateDuration(targetValue);
            int frames = 80;
            int delay = durationMs / frames;
            double progress = 0;

            StartFadeInAnimation(textBlock, durationMs);

            for (int i = 0; i <= frames; i++)
            {
                progress = (double)i / frames;
                double easedProgress = EaseOutQuart(progress);

                int current = (int)(targetValue * easedProgress);
                textBlock.Text = current.ToString();

                await Task.Delay(delay);
            }

            StartBounceAnimation(textBlock);
        }

        private async void AnimateDecimal(TextBlock textBlock, decimal targetValue)
        {
            int durationMs = CalculateDuration((int)targetValue);
            int frames = 80;
            int delay = durationMs / frames;
            double progress = 0;

            StartFadeInAnimation(textBlock, durationMs);

            for (int i = 0; i <= frames; i++)
            {
                progress = (double)i / frames;
                double easedProgress = EaseOutQuart(progress);

                decimal current = targetValue * (decimal)easedProgress;
                textBlock.Text = $"{current:C0}";

                await Task.Delay(delay);
            }

            StartBounceAnimation(textBlock);
        }

        private void StartFadeInAnimation(TextBlock textBlock, int durationMs)
        {
            textBlock.Opacity = 0;

            var fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(durationMs + 200),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            textBlock.BeginAnimation(OpacityProperty, fadeAnimation);
        }

        private void StartBounceAnimation(TextBlock textBlock)
        {
            var bounceAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.1,
                AutoReverse = true,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new BounceEase
                {
                    Bounces = 1,
                    Bounciness = 2,
                    EasingMode = EasingMode.EaseOut
                }
            };

            textBlock.RenderTransformOrigin = new Point(0.5, 0.5);
            textBlock.RenderTransform = new ScaleTransform(1.0, 1.0);

            textBlock.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, bounceAnimation);
            textBlock.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, bounceAnimation);
        }

        private double EaseOutQuart(double t) => 1 - Math.Pow(1 - t, 4);

        private int CalculateDuration(int value)
        {
            if (value < 50) return 600;
            if (value < 500) return 900;
            if (value < 5000) return 1400;
            
            return 1700;
        }

        #endregion
    }
}
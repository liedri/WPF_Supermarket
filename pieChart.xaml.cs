using Supermarket.BLBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using LiveCharts.Defaults;
using System.IO;
using System.Drawing.Imaging;
using Accord;
using Accord.Math;

namespace Supermarket
{

    public class RuleView
    {
        public Product Prod1 { get; set; }
        public ImageSource image1 { get; set; }
        public Product Prod2 { get; set; }
        public ImageSource image2 { get; set; }
        public double Confidence { get; set; }

        public RuleView(Product x, Product y, double c)
        {
            Prod1 = x;
            Prod2 = y;
            Confidence = c;
            string str1 = Prod1.Image;
            Byte[] s = Convert.FromBase64String(str1);
            var ms = new MemoryStream(s);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            image1 = bitmapImage;

            string str2 = Prod2.Image;

            s = Convert.FromBase64String(str2);
            ms = new MemoryStream(s);
            image = System.Drawing.Image.FromStream(ms);
            ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            image2 = bitmapImage;
        }
    }

    /// <summary>
    /// Interaction logic for pieChart.xaml
    /// </summary>
    /// 

    public partial class pieChart : UserControl
    {
        Model.Model model = new Model.Model();
        List<StoreClass> prods1 = new List<StoreClass>();
        List<StoreClass> prods2 = new List<StoreClass>();
        List<StoreClass> prods3 = new List<StoreClass>();
        float[,] prods4 = new float[5, 7];

        public pieChart()
        {
            InitializeComponent();
            pieBorder.Visibility = Visibility.Collapsed;
            tableBorder.Visibility = Visibility.Collapsed;
            colBorder.Visibility = Visibility.Collapsed;
            PurchaseStatistics.MouseLeftButtonUp += showPurchaseStatistics;
            PieChart.MouseLeftButtonUp += showPieChart;
            ColumnChart.MouseLeftButtonUp += showColChart;
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }

        public SeriesCollection SeriesCollection { get; set; }

        public void showPurchaseStatistics(object sender, MouseButtonEventArgs e)
        {
            pieBorder.Visibility = Visibility.Collapsed;
            tableBorder.Visibility = Visibility.Visible;
            colBorder.Visibility = Visibility.Collapsed;
            List<RuleView> rvs = new List<RuleView>();
            List<Rule> rules = model.ML();

            foreach (var rule in rules)
            {
                rvs.Add(new RuleView(rule.Prod1, rule.Prod2, rule.Confidence));
            }

            DataContext = rvs;
        }

        public void showPieChart(object sender, MouseButtonEventArgs e)
        {
            pieBorder.Visibility = Visibility.Visible;
            tableBorder.Visibility = Visibility.Collapsed;
            colBorder.Visibility = Visibility.Collapsed;
        }

        public void showColChart(object sender, MouseButtonEventArgs e)
        {
            pieBorder.Visibility = Visibility.Collapsed;
            tableBorder.Visibility = Visibility.Collapsed;
            colBorder.Visibility = Visibility.Visible;
            prods4 = model.getStoreClass2();
          
            SeriesCollection = new SeriesCollection();
            for (int i = 0; i < prods4.Rows(); i++)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = ((CATEGORY)i).ToString(),
                    Values = new ChartValues<float>(prods4.GetRow(i)),
                    PointGeometry = null
                });
            }
            Labels = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList().Select(c => c.ToString()).ToArray();
            YFormatter = value => value.ToString("C");

            DataContext = this;
            colChart.Series = SeriesCollection;
            colY.LabelFormatter = YFormatter;
        }
    
    
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
        private void chooseComboBox(object sender, SelectionChangedEventArgs e)
        {
          
            if (comboBox.SelectedItem != null)
            {
                if (comboBox.SelectedItem == byProduct)
                {
                    if (prods1.Count == 0)
                    {
                        prods1 = model.getProductsAmount();
                    }
                   
                    SeriesCollection = new SeriesCollection();
                    foreach (var prod in prods1)
                    {
                        SeriesCollection.Add(new PieSeries
                        {
                            Title = prod.title,
                            Values = new ChartValues<ObservableValue> { new ObservableValue(prod.number) },
                            DataLabels = true
                        });
                    }

                    DataContext = this;
                    Chart.Series = SeriesCollection;
                }
                else if (comboBox.SelectedItem == byCategory)
                {
                    if (prods2.Count == 0)
                    {
                        prods2 = model.getCategoryAmount();
                    }

                    SeriesCollection = new SeriesCollection();
                    foreach (var prod in prods2)
                    {
                        SeriesCollection.Add(new PieSeries
                        {
                            Title = prod.title,
                            Values = new ChartValues<ObservableValue> { new ObservableValue(prod.number) },
                            DataLabels = true
                        });
                    }

                    DataContext = this;
                    Chart.Series = SeriesCollection;

                }
                else if (comboBox.SelectedItem == byWeek)
                {
                    if (prods3.Count == 0)
                    {
                        prods3 = model.WeeklyAmount();
                    }

                    SeriesCollection = new SeriesCollection();
                    foreach (var prod in prods3)
                    {
                        SeriesCollection.Add(new PieSeries
                        {
                            Title = prod.title,
                            Values = new ChartValues<ObservableValue> { new ObservableValue(prod.number) },
                            DataLabels = true
                        });
                    }

                    DataContext = this;
                    Chart.Series = SeriesCollection;

                }
            }
        }

    }
}


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
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
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Supermarket.BLBE;
using Supermarket.Model;

namespace Supermarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Model.Model model = new Model.Model();
        public bool IsAddPurchase = false;
        public UserControl Current = null;

        public MainWindow()
        {
            InitializeComponent();
            BackHomeB.Visibility = Visibility.Collapsed;
        }

        private void BackHome(object sender, RoutedEventArgs e)
        {
            if(IsAddPurchase == true)
            {
                var res = MessageBox.Show("?אם תצא לפני סיום הקניה הפרטים לא ישמרו.\n האם אתה בטוח שברצונך לעזוב", "smart Shop", MessageBoxButton.YesNo);
                if(res == MessageBoxResult.No)
                {
                    return;
                }
                IsAddPurchase = false;
            }
            MainGrid.Children.Remove(Current);
            StatisticsButton.Visibility = Visibility.Visible;
            //RecommendationsButton.Visibility = Visibility.Visible;
            AddProductButton.Visibility = Visibility.Visible;
            BackHomeB.Visibility = Visibility.Collapsed;

        }
        
        private void MoveToPrepare()
        {
            BackHomeB.Visibility = Visibility.Visible;
            StatisticsButton.Visibility = Visibility.Collapsed;
            //RecommendationsButton.Visibility = Visibility.Collapsed;
            AddProductButton.Visibility = Visibility.Collapsed;
        }

        private void MoveToAddPuchase(object sender, RoutedEventArgs e)
        {
            IsAddPurchase = true;
            Current = new AddPurchase();
            MainGrid.Children.Add(Current);
            Grid.SetRow(Current, 1);
            MoveToPrepare();
        }

        private void MoveToRecommendations(object sender, RoutedEventArgs e)
        {
            Current = new Reference();
            MainGrid.Children.Add(Current);
            Grid.SetRow(Current, 1);
            MoveToPrepare();
        }

        private void MoveToStatistics(object sender, RoutedEventArgs e)
        {
            Current = new pieChart();
            MainGrid.Children.Add(Current);
            Grid.SetRow(Current, 1);
            MoveToPrepare();
        }

        private void MoveToAddProduct(object sender, RoutedEventArgs e)
        {
            Current = new addProduct();
            MainGrid.Children.Add(Current);
            Grid.SetRow(Current, 1);
            MoveToPrepare();
        }

        private void MoveToCatalog(object sender, RoutedEventArgs e)
        {
            Current = new catalog();
            MainGrid.Children.Add(Current);
            Grid.SetRow(Current, 1);
            Grid.SetRowSpan(Current, 2);
            MoveToPrepare();
        }
    }
}

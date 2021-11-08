using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
using Microsoft.Win32;
using Supermarket.Model;

namespace Supermarket
{
    /// <summary>
    /// Interaction logic for AddPurchase.xaml
    /// </summary>
    public partial class AddPurchase : UserControl
    {
        ObservableCollection<ProductView> products = new ObservableCollection<ProductView>();

        ProductView Current = null;

        Model.Model model = new Model.Model();
        public AddPurchase()
        {
            InitializeComponent();
            products.CollectionChanged += products_CollectionChanged;
            DataContext = products;
            prodList.ItemsSource = products;
        }

        void products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataContext = products;
            prodList.ItemsSource = products;
        }

        public string filepathQRImage { get; set; }

        private BitmapImage helper(System.Drawing.Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void UploadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select QR Image";
            ofd.Filter = "Image Files(*.png) | *.png";
            if (ofd.ShowDialog() == true)
            {
                Current = model.QRtoPV(ofd.FileName, 0);
                QRImage.Source = helper(model.ExtractImageProduct(Current.ProdId));
                Current.filePath = QRImage.Source;
                filepathQRImage = ofd.FileName;
            }
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            ProductView removeItem = (ProductView)((Button)sender).DataContext;
            products.Remove(removeItem);
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            Current.amount = float.Parse(amount.Text);
            products.Add(Current);
            QRImage.Source = null;
            amount.Text = "";
            Current = null;
        }


        private void finish(object sender, RoutedEventArgs e)
        {
            List<ProductView> prods = products.ToList<ProductView>();
            model.AddNewPurchaes(prods);
            MessageBox.Show("הקניה התווספה בהצלחה", "Smart Shop");
            products = new ObservableCollection<ProductView>();
            prodList.ItemsSource = products;
        }

    }
}

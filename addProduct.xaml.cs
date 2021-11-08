using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Supermarket
{
    /// <summary>
    /// Interaction logic for addProduct.xaml
    /// </summary>
    public partial class addProduct : UserControl
    {

        public Model.Model model = new Model.Model();

        public addProduct()
        {
            InitializeComponent();
        }

        public string filepathtemp12 { get; set; }

        private void loadtemp12(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select QR Image";
            ofd.Filter = "Image Files(*.png) | *.png";
            if (ofd.ShowDialog() == true)
            {
                temp12.Source = new BitmapImage(new Uri(ofd.FileName));
                filepathtemp12 = ofd.FileName;
            }
        }

        private void loadtemp22(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "Image Files(*.png) | *.png";
            if (ofd.ShowDialog() == true)
            {
                temp22.Source = new BitmapImage(new Uri(ofd.FileName));

            }
        }

        private void addPTemp(object sender, RoutedEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            System.Windows.Media.Imaging.BmpBitmapEncoder bbe = new BmpBitmapEncoder();
            bbe.Frames.Add(BitmapFrame.Create(new Uri(temp22.Source.ToString(), UriKind.RelativeOrAbsolute)));

            bbe.Save(ms);
            System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms);
            model.AddP(filepathtemp12, img2);

            temp22.Source = null;
            temp12.Source = null;
        }

        private void Do_Nothing(object sender, RoutedEventArgs e)
        {

        }
    }
}

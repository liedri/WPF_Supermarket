#pragma warning disable 0649

using Supermarket.BLBE;
using System;
using System.Collections.Generic;
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

namespace Supermarket
{

    public class ProductCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public ImageSource Image { get; set; }
        public CATEGORY Category { get; set; }

        public ProductCatalog(int id, string name, float price, string str, CATEGORY category)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;

            Byte[] s = Convert.FromBase64String(str);
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
            Image = bitmapImage;

        }
    }


    /// <summary>
    /// Interaction logic for catalog.xaml
    /// </summary>
    public partial class catalog : UserControl
    {
        //List<ProductCard> cards;
        //MyControl.DataContext = MenuItems;
        Model.Model model = new Model.Model();
        public List<ProductCatalog> products = new List<ProductCatalog>();
        public catalog()
        {
            InitializeComponent();

            foreach (var v in model.getAllProducts())
            {
                products.Add(new ProductCatalog(v.Id, v.Name, v.Price, v.Image, v.Category));
            }

            if (products.Count > 0)
                ListViewProducts.ItemsSource = products;
            All.MouseLeftButtonUp += showAll;
            Vegetables.MouseLeftButtonUp += showVegetables;
            Fruit.MouseLeftButtonUp += showFruit;
            Drink.MouseLeftButtonUp += showDrinks;
            Miscellaneous.MouseLeftButtonUp += showMiscellaneous;
            Toiletries.MouseLeftButtonUp += showToiletries;

            /*cards = new List<ProductCard>();
            List<Product> prods = model.getAllProducts();
            foreach(var prod in prods)
            {
                ProductCard uc = new ProductCard(prod);
                cards.Add(uc);
                uc.Children.Add(uc);
            }
            */
        }

        public void showAll(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> all = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                    if (prod.Price <= priceSlider.Value || priceSlider.Value == 0)
                        all.Add(prod);
            ListViewProducts.ItemsSource = all;
        }

        public void showVegetables(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> vegetables = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                {
                    if (prod.Category.Equals(CATEGORY.vegetables) && (prod.Price <= priceSlider.Value || priceSlider.Value == 0))
                        vegetables.Add(prod);
                }
            ListViewProducts.ItemsSource = vegetables;
        }

        public void showFruit(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> fruit = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                {
                    if (prod.Category.Equals(CATEGORY.fruit) && (prod.Price <= priceSlider.Value || priceSlider.Value == 0))
                        fruit.Add(prod);
                }
            ListViewProducts.ItemsSource = fruit;
        }
        public void showDrinks(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> drinks = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                {
                    if (prod.Category.Equals(CATEGORY.drink) && (prod.Price <= priceSlider.Value || priceSlider.Value == 0))
                        drinks.Add(prod);
                }
            ListViewProducts.ItemsSource = drinks;
        }
        public void showMiscellaneous(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> miscellaneous = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                {
                    if (prod.Category.Equals(CATEGORY.Miscellaneous) && (prod.Price <= priceSlider.Value || priceSlider.Value == 0))
                        miscellaneous.Add(prod);
                }
            ListViewProducts.ItemsSource = miscellaneous;
        }

        public void showToiletries(object sender, MouseButtonEventArgs e)
        {
            List<ProductCatalog> toiletries = new List<ProductCatalog>();
            if (products.Count > 0)
                foreach (var prod in products)
                {
                    if (prod.Category.Equals(CATEGORY.toiletries) && (prod.Price <= priceSlider.Value || priceSlider.Value == 0))
                        toiletries.Add(prod);
                }
            ListViewProducts.ItemsSource = toiletries;
        }

        private void priceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (priceSlider != null)
            {
                PriceValue.Text = priceSlider.Value.ToString();
            }

        }

    }
}

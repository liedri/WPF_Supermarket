using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Supermarket.BL;
using Supermarket.BLBE;

namespace Supermarket.Model
{

    public class ProductView
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public float amount { get; set; }
        public ImageSource filePath { get; set; } 

        public ProductView(int pid, string name, float a, string fp)
        {
            ProdId = pid;
            ProdName = name;
            amount = a;
            filePath = new BitmapImage(new Uri(fp));
        }
    }


    public class Model
    {
        IBL _bl;

        public Model()
        {
            _bl = new BL.BL();
        }

        public Image ExtractImage(string str)
        { 
            Byte[] s = Convert.FromBase64String(str);
            var ms = new MemoryStream(s);
            return Image.FromStream(ms);
        }


        public ProductView QRtoPV(string filePath, float a)
        {
            Product p = _bl.TextToProduct(filePath);
            ProductView pv = new ProductView(p.Id, p.Name, a, filePath);
            return pv;
        }

        public void AddP(string qr, Image i2)
        {
            Product p = _bl.TextToProduct(qr);
            var ms = new MemoryStream();
            i2.Save(ms, ImageFormat.Png);
            byte[] a = ms.GetBuffer();
            p.Image = Convert.ToBase64String(a);
            AddNewProduct(p);
        }


        public Image ExtractImageProduct(int id)
        {
            Product p = _bl.RetrieveProduct(id);
            Byte[] s = Convert.FromBase64String(p.Image);
            var ms = new MemoryStream(s);
            return Image.FromStream(ms);
        }

        public List<StoreClass> getCategoryAmount()
        {
            return _bl.CategoryAmount();
        }
        public List<StoreClass> getProductsAmount()
        {
            return _bl.ProductsAmount();
        }

        public List<StoreClass> WeeklyAmount()
        {
            return _bl.WeeklyAmount();
        }

        public List<Rule> ML()
        {
            return _bl.ML();
        }
        public float[,] getStoreClass2()
        {
            return _bl.getStoreClass2();
        }

        #region Products

        public List<Product> getAllProducts()
        {
            return _bl.AllProducts();
        }

        public void AddNewProduct(Product product)
        {
            _bl.AddNewProduct(product);
        }


        public Product RetrieveProduct(int id)
        {
            return _bl.RetrieveProduct(id);
        }

        public void UpdateProduct(Product product)
        {
            _bl.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _bl.DeleteProduct(id);
        }

        #endregion


        #region Purchaes
        public void AddNewPurchaes(Purchase p)
        {
            _bl.AddNewPurchaes(p);
        }

        public Purchase RetrievePuchase(int id)
        {
            return _bl.RetrievePuchase(id);
        }

        public void UpdatePurchase(Purchase p)
        {
            _bl.UpdatePurchase(p);
        }

        public void DeletePurchase(int id)
        {
            _bl.DeletePurchase(id);
        }

        public void AddNewPurchaes(List<ProductView> prods)
        {
            List<ProdAmount> prodamounts = new List<ProdAmount>();
            foreach(var prod in prods)
            {
                prodamounts.Add(new ProdAmount(prod.ProdId, prod.amount));
            }
            Purchase p = new Purchase(DateTime.Now, prodamounts);
            AddNewPurchaes(p);
        }

        #endregion
    }
}

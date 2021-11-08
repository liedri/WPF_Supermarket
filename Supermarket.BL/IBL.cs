using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.BLBE;
using Supermarket.DAL;

namespace Supermarket.BL
{
    public interface IBL
    {
        Product TextToProduct(string str);

        #region Products

        void AddNewProduct(Product product);

        Product RetrieveProduct(int id);

        void UpdateProduct(Product product);

        void DeleteProduct(int id);

        List<Product> AllProducts();

        List<StoreClass> ProductsAmount();

        List<StoreClass> CategoryAmount();

        List<StoreClass> WeeklyAmount();

        List<Rule> ML();
        float[,] getStoreClass2();

        #endregion


        #region Purchaes
        void AddNewPurchaes(Purchase p);

        Purchase RetrievePuchase(int id);

        void UpdatePurchase(Purchase p);

        void DeletePurchase(int id);

        #endregion
    }
}

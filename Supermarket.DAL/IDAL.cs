using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.BLBE;

namespace Supermarket.DAL
{
    public interface IDAL
    {

        #region Products

        void AddNewProduct(Product product);

        Product RetrieveProduct(int id);

        void UpdateProduct(Product product);

        void DeleteProduct(int id);

        List<Product> AllProducts();

        #endregion


        #region Purchaes
        void AddNewPurchaes(Purchase p);

        Purchase RetrievePuchase(int id);

        void UpdatePurchase(Purchase p);

        void DeletePurchase(int id);

        List<Purchase> AllPurchases();

        #endregion


    }
}

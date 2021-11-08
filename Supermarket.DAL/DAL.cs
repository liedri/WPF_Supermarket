using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using Supermarket.BLBE;


namespace Supermarket.DAL
{
    public class DAL: IDAL
    {
        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "rMlO5LOcRkSwhS4Fuk3v5R9CL7KEHdXUlz4daI6M",
            BasePath = "https://projectwpf2021-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        public DAL()
        {
            try
            {
                client = new FirebaseClient(ifc);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        #region Products

        public async void AddNewProduct(Product product)
        {
            SetResponse rsp = await client.SetAsync<Product>("Products/" + product.Id.ToString(), product);
            Product result = rsp.ResultAs<Product>(); 
        }

        public Product RetrieveProduct(int id)
        {
            FirebaseResponse rsp = client.Get("Products/" + id.ToString());
            Product result = rsp.ResultAs<Product>();
            return result;
        }

        public async void UpdateProduct(Product product)
        {
            FirebaseResponse rsp = await client.UpdateAsync<Product>("Products/" + product.Id.ToString(), product);
            Product result = rsp.ResultAs<Product>();
        }

        public async void DeleteProduct(int id)
        {
            FirebaseResponse rsp = await client.DeleteAsync("Products/" + id.ToString());
        }

        #endregion


        #region Purchaes
        public async void AddNewPurchaes(Purchase p)
        {
            SetResponse rsp = await client.SetAsync<Purchase>("Purchases/" + p.Id.ToString(), p);
            Purchase result = rsp.ResultAs<Purchase>();
        }

        public Purchase RetrievePuchase(int id)
        {
            FirebaseResponse rsp = client.Get("Purchases/" + id.ToString());
            Purchase result = rsp.ResultAs<Purchase>();
            return result;
        }

        public async void UpdatePurchase(Purchase p)
        {
            FirebaseResponse rsp = await client.UpdateAsync<Purchase>("Purchases/" + p.Id.ToString(), p);
            Purchase result = rsp.ResultAs<Purchase>();
        }

        public async void DeletePurchase(int id)
        {
            FirebaseResponse rsp = await client.DeleteAsync("Purchases/" + id.ToString());
        }

        #endregion


        public List<Product> AllProducts()
        {
            FirebaseResponse response = client.Get("Products");
            Dictionary<string, Product> data = response.ResultAs<Dictionary<string, Product>>();

            return data.Values.ToList();

        }


        public List<Purchase> AllPurchases()
        {
            FirebaseResponse response = client.Get("Purchases");
            Dictionary<string, Purchase> data = response.ResultAs<Dictionary<string, Purchase>>();

            return data.Values.ToList();

        }





    }
}

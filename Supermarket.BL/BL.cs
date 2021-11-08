using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.DAL;
using Supermarket.BLBE;
using System.Drawing;
using Accord.MachineLearning.Rules;
using System.Linq;
using System.ComponentModel;

namespace Supermarket.BL
{
    public class BL : IBL
    {
        IDAL dal;

        public BL()
        {
            dal = new DAL.DAL();
        }

        public Product TextToProduct(string str)
        {
            QRCodeFetcher f = new QRCodeFetcher();
            Image i = Image.FromFile(str);
            str = f.Fetcher(i);
            string[] arr = str.Split(',');
            return new Product(Int32.Parse(arr[0]), arr[1], float.Parse(arr[2]), "", (CATEGORY)Enum.Parse(typeof(CATEGORY), arr[3]));
        }

        #region Products

        public void AddNewProduct(Product product)
        {
            dal.AddNewProduct(product);
        }

        public Product RetrieveProduct(int id)
        {
            return dal.RetrieveProduct(id);
        }

        public void UpdateProduct(Product product)
        {
            dal.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            dal.DeleteProduct(id);
        }

        public List<Product> AllProducts()
        {
            return dal.AllProducts();
        }

        #endregion


        #region Purchaes
        public void AddNewPurchaes(Purchase p)
        {
            dal.AddNewPurchaes(p);
        }

        public Purchase RetrievePuchase(int id)
        {
            return dal.RetrievePuchase(id);
        }

        public void UpdatePurchase(Purchase p)
        {
            dal.UpdatePurchase(p);
        }

        public void DeletePurchase(int id)
        {
            dal.DeletePurchase(id);
        }

        #endregion

        private AssociationRule<int>[] ML_(SortedSet<int>[] dataset)
        {
            Apriori apriori = new Apriori(threshold: 2, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            // The result should be:
            // 
            //   new int[][]
            //   {
            //       new int[] { 4 },
            //       new int[] { 3 }
            //   };

            // Meaning the most likely product to go alongside the products
            // being bought is item 4, and the second most likely is item 3.

            // We can also obtain the association rules from frequent itemsets:
            AssociationRule<int>[] rules = classifier.Rules;

            return rules;
        }


        public List<Rule> ML()
        {
            List<SortedSet<int>> dataset = new List<SortedSet<int>>();
            foreach (var purchase in dal.AllPurchases())
            {
                SortedSet<int> set = new SortedSet<int>(purchase.Products.Select(pa => pa.ProdId));
                dataset.Add(set);
            }
            var res = ML_(dataset.ToArray());

            List<Rule> rules = new List<Rule>();

            foreach (var rule in res)
            {
                rules.Add(new Rule(dal.RetrieveProduct(rule.X.First()), dal.RetrieveProduct(rule.Y.First()), rule.Confidence));
            }
            return rules;
        }

        public List<StoreClass> ProductsAmount()
        {
            List<StoreClass> result = new List<StoreClass>();
            List<Purchase> puchases = dal.AllPurchases();
            Dictionary<int, float> dict = new Dictionary<int, float>();
            foreach(var purchase in puchases)
            {
                foreach(var prodAmount in purchase.Products)
                {
                    if (!dict.ContainsKey(prodAmount.ProdId))
                    {
                        dict.Add(prodAmount.ProdId, prodAmount.Amount);
                    }
                    else
                    {
                        dict[prodAmount.ProdId] += prodAmount.Amount;
                    }
                }
            }
            foreach(var e in dict)
            {
                result.Add(new StoreClass(dal.RetrieveProduct(e.Key).Name, e.Value));
            }
            return result;
        }


        public List<StoreClass> CategoryAmount()
        {
            List<StoreClass> result = new List<StoreClass>();
            List<Purchase> puchases = dal.AllPurchases();
            Dictionary<int, float> dict = new Dictionary<int, float>();
            foreach (var purchase in puchases)
            {
                foreach (var prodAmount in purchase.Products)
                {
                    if (!dict.ContainsKey(prodAmount.ProdId))
                    {
                        dict.Add(prodAmount.ProdId, prodAmount.Amount);
                    }
                    else
                    {
                        dict[prodAmount.ProdId] += prodAmount.Amount;
                    }
                }
            }

            Dictionary<int, float> cdict = new Dictionary<int, float>();

            foreach (var e in dict)
            {
                Product p = dal.RetrieveProduct(e.Key);
                if (!cdict.ContainsKey((int)p.Category))
                {
                    cdict.Add((int)p.Category, e.Value);
                }
                else
                {
                    cdict[(int)p.Category] += e.Value;
                }
            }

            foreach (var e in cdict)
            {
                result.Add(new StoreClass(((CATEGORY)e.Key).ToString(), e.Value));
            }

            return result;
        }

        public List<StoreClass> WeeklyAmount()
        {
            List<StoreClass> result = new List<StoreClass>();
            List<Purchase> puchases = dal.AllPurchases();
            Dictionary<int, float> dict = new Dictionary<int, float>();
            foreach (var purchase in puchases)
            {
                if (!dict.ContainsKey((int)purchase.PurchaseDate.DayOfWeek))
                {
                    dict.Add((int)purchase.PurchaseDate.DayOfWeek, purchase.Products.Count);
                }
                else
                {
                    dict[(int)purchase.PurchaseDate.DayOfWeek] += purchase.Products.Count;
                }
            }

            foreach (var e in dict)
            {
                result.Add(new StoreClass(Enum.GetName(typeof(DayOfWeek), e.Key).ToString(), e.Value));
            }

            return result;
        }

        public float[,] getStoreClass2()
        {
            List<Product> ps = dal.AllProducts();
            Dictionary<int, Product> dict = new Dictionary<int, Product>();
            foreach (var p in ps)
            {
                dict.Add(p.Id, p);
            }
            List<StoreClass2> result = new List<StoreClass2>();
            List<Purchase> puchases = dal.AllPurchases();
            float[,] array = new float[5, 7];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    array[i, j] = 0;
                }
            }
            foreach (var purchase in puchases)
            {
                foreach (var prodAmount in purchase.Products)
                {
                    array[(int)dict[prodAmount.ProdId].Category, (int)purchase.PurchaseDate.DayOfWeek] += prodAmount.Amount;
                }
            }
            return array;
        }
    }
}

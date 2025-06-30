using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listProducts = context.Products.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving products: " + e.Message, e);
            }
            return listProducts;

        }
        public static Product FindProductById(int proId)
        {
            Product p = new Product();
            try
            {
                using (var context = new MyDbContext())
                {
                    p = context.Products.SingleOrDefault(x => x.ProductId == proId);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error finding product by ID: " + e.Message, e);
            }
            return p;
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using(var context = new MyDbContext())
                {
                    context.Products.Add(p);    
                    context.SaveChanges();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error saving product: " + e.Message, e);
            }   
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using(var context = new MyDbContext())
                {
                    var p1 = context.Products.SingleOrDefault(x => x.ProductId == p.ProductId); 
                    context.Products.Remove(p1);
                    context.SaveChanges();
                }   
            }
            catch(Exception e)
            {
                throw new Exception("Error deleting product: " + e.Message, e);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error updating product: " + e.Message, e);
            }
        }
    }
}

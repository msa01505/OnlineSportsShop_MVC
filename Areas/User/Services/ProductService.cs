using Proj.Data;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Areas.User.Services
{
    public class ProductService:IProduct
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ProductService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public List<Product> GetProducts()
        {
            return applicationDbContext.Products.ToList();
        }
        public List<Product> GetClothes()
        {
            return GetProducts().Where(t => t.Type == ProductType.Clothes).ToList();
        }
        public List<Product> GetShoes()
        {
            return GetProducts().Where(t => t.Type == ProductType.Shoes).ToList();
        }
        public List<Product> GetWatches()
        {
            return GetProducts().Where(t => t.Type == ProductType.SmartWatches).ToList();
        }
        public List<Product> GetMachines()
        {
            return GetProducts().Where(t => t.Type == ProductType.Machines).ToList();
        }
        public Product GetProduct(int id, ProductType type)
        {
            Product Product = GetProducts().FirstOrDefault(s => s.ID == id && s.Type == type);
            return Product;
        }
    }
}

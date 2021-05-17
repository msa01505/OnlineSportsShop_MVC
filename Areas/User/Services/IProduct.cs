using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Areas.User.Services
{
    public interface IProduct
    {
        public List<Product> GetProducts();
        public List<Product> GetClothes();
        public List<Product> GetShoes();
        public List<Product> GetWatches();
        public List<Product> GetMachines();
        public Product GetProduct(int id, ProductType Type);

    }
}

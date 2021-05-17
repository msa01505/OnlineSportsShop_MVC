using Proj.Data;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Areas.Admin.Services
{
    public class Product_Service : CRUDRepo<Product>


    {
        public Product_Service(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public void Delete(int id)
        {
            var x = Context.Products.FirstOrDefault(x => x.ID == id);
            Context.Products.Remove(x);
            Context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            return Context.Products.ToList();
        }

        public Product GetDetails(int id)
        {
            return Context.Products.FirstOrDefault(x => x.ID == id);
        }

        public void Insert(Product obj)
        {
            Context.Products.Add(obj);
            Context.SaveChanges();
        }

        public void Update(Product obj)
        {
            Context.Products.Update(obj);
            Context.SaveChanges();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Areas.Admin.Services
{
  public  interface CRUDRepo<T>
    {
        public List<T> GetAll();
        public T GetDetails(int id);
        public void Update(T obj);
        public void Insert(T obj);
        public void Delete(int id);
    }
}

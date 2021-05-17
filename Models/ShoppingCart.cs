using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ID { get; set; }
       
        public virtual ICollection<ShoppingCartItem> Items { get; set; }

        //put when register new user one -> one 
        [ForeignKey("AspNetUsers")]
        public string User_ID { get; set; }

    }
}

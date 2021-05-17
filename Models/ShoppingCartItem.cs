using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int ID { get; set; }
        public int TotalAmount { get; set; }
        public Product ProductItem { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartID { get; set; }
        public virtual ShoppingCart cartList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public decimal TotalPrice { get; set; }

        [ForeignKey("AspNetUsers")]
        public string User_ID { get; set; }
        public Address Order_Adderss { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public string PaymentWay { get; set; }
        
    }
}

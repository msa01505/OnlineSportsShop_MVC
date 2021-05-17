using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Models
{
    public class User : IdentityUser
    {
        
        public Gender Gender { get; set; }

        public int Age { get; set; }
        public Address Address { get; set; }
         
        public ShoppingCart ShoppingCart { get; set; }

    }
}

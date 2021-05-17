using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Models
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "please enter a name !!!")]
        [MaxLength(30, ErrorMessage = "too long name !!!!")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Enter ur email !!!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

       
    }
}

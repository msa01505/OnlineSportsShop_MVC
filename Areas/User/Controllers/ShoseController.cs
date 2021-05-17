using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Proj.Models;
using Proj.Areas.User.Services;
using OnlineSportShop.Areas.User.Services;
using Microsoft.AspNetCore.Identity;

namespace OnlineSportShop.Areas.User.Controllers
{
    [Area("User")]
    public class ShoseController : Controller
    {
        private readonly IProduct shose;
        private readonly ICartList CartList;
        private readonly UserManager<Proj.Models.User> userManager;
        private string loggedUser;
        public ShoseController(IProduct shose, ICartList CartList,
            UserManager<Proj.Models.User> userManager
            )
        {
            this.userManager = userManager;
            this.CartList = CartList;
            this.shose = shose;
        }

        //to git all product List
        public IActionResult Index(int ?id, string color, string gender)
        {
            ViewData["ColorFilter"] = color;
            ViewData["GenderFilter"] = gender;

            List<Product> shoseProductList = new List<Product>();
            
            if (color != null && gender == null)
            {
                shoseProductList = shose.GetShoes().Where(l => l.Color.ToLower() == color.ToLower()).ToList();
            }
            else if(gender != null && color == null)
            {
                shoseProductList = shose.GetShoes().Where(l => l.Gender.ToString().ToLower() == gender.ToLower()).ToList();
            }else if (color!=null && gender!=null) {
                shoseProductList = shose.GetShoes().Where(
                    l => (l.Gender.ToString().ToLower() == gender.ToLower()) && (l.Color.ToLower() == color.ToLower()) ).ToList();
            }
            else
            {
                shoseProductList = shose.GetShoes();
            }
            ViewBag.shoseProductList = shoseProductList;
            
            return View();
        }
        public IActionResult getShoseProduct(int id)
        {
            Product _shose = shose.GetProduct(id,ProductType.Shoes);
            ViewBag._shose = _shose;
            return View();
        }
        public IActionResult AddToCart(int id)
        {
            loggedUser = this.userManager.GetUserId(HttpContext.User);
            if (loggedUser != null)
            {
                var productItem = shose.GetProduct(id, ProductType.Shoes);
                CartList.AddToCart(productItem, loggedUser);
                return RedirectToAction("Index");
            }
            else
            {

                return Redirect("https://localhost:44363/Identity/Account/Login");

            }
        }
    }
}

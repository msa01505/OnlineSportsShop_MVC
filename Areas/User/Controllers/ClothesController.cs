using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Proj.Areas.User.Services;
using Proj.Models;
using OnlineSportShop.Areas.User.Services;
using Microsoft.AspNetCore.Identity;

namespace OnlineSportShop.Areas.User.Controllers
{
    [Area("User")]
    public class ClothesController : Controller
    {
        private readonly IProduct Clothes;
        private readonly ICartList CartList;
        private readonly UserManager<Proj.Models.User> userManager;
        private string loggedUser;

        List<Product> ClothesList = new List<Product>();
        public ClothesController(IProduct Clothes, ICartList CartList,
            UserManager<Proj.Models.User> userManager
            )
        {
            this.userManager = userManager;
            this.CartList = CartList;
            this.Clothes = Clothes;
        }
        public IActionResult Index()
        {
            ClothesList = Clothes.GetClothes();
            ViewBag.ClothesList = ClothesList;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string color, string gender)
        {
            ViewData["ColorFilter"] = color;
            ViewData["GenderFilter"] = gender;
            if (color != null && gender == null)
            {
                ClothesList = Clothes.GetClothes().Where
                    (l => l.Color.ToLower() == color.ToLower()).ToList();
            }
            else if (gender != null && color == null)
            {
                ClothesList = Clothes.GetClothes().Where
                    (l => l.Gender.ToString().ToLower() == gender.ToLower()).ToList();
            }
            else if (color != null && gender != null)
            {
                ClothesList = Clothes.GetClothes().Where
                    (l => (l.Gender.ToString().ToLower() == gender.ToLower()) &&
                    (l.Color.ToLower() == color.ToLower())).ToList();
            }
            else if (color == null && gender == null)
            {
                ClothesList = Clothes.GetClothes();
            }
            ViewBag.ClothesList = ClothesList;
            return View();
        }
        public IActionResult Details(int id)
        {
            var Cloth = Clothes.GetProduct(id, ProductType.Clothes);
            ViewBag.Cloth = Cloth;
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            loggedUser = this.userManager.GetUserId(HttpContext.User);
            if (loggedUser != null)
            {
                var productItem = Clothes.GetProduct(id, ProductType.Clothes);
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

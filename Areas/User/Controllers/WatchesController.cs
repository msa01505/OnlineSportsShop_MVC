using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Areas.User.Services;
using Proj.Areas.User.Services;
using Proj.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineSportShop.Areas.User.Controllers
{
    [Area("User")]
    public class WatchesController : Controller
    {
        private readonly IProduct Watches;
        private readonly ICartList CartList;
        private readonly UserManager<Proj.Models.User> userManager;
        private string loggedUser;

        List<Product> WatchesList = new List<Product>();
        public WatchesController(IProduct Watches, ICartList CartList,
            UserManager<Proj.Models.User> userManager
            )
        {
            this.userManager = userManager;
            this.CartList = CartList;
            this.Watches = Watches;
            WatchesList = Watches.GetWatches();
        }
        public IActionResult Index()
        {
            ViewBag.WatchesList = WatchesList;
            return View();
        }
   
        [HttpPost]
        public ActionResult Index(string color, decimal price)
        {
            ViewData["ColorFilter"] = color;
            ViewData["PriceFilter"] = price;
            if (color != null && price == 0)
            {
                WatchesList = Watches.GetWatches().Where
                    (l => l.Color.ToLower() == color.ToLower()).ToList();
            }
            else if (price != 0 && color == null)
            {
                WatchesList = Watches.GetWatches().Where
                    (l => l.Price <= price).ToList();
            }
            else if (color != null && price != 0)
            {
                WatchesList = Watches.GetWatches().Where
                    (l => (l.Price <= price) &&
                    (l.Color.ToLower() == color.ToLower())).ToList();
            }
            else if (color == null && price == 0)
            {
                WatchesList = Watches.GetWatches();
            }
            ViewBag.WatchesList = WatchesList;
            return View();
        }
        public IActionResult Details(int id)
        {
            var Watch = Watches.GetProduct(id,ProductType.SmartWatches);
            ViewBag.Watch = Watch;
            return View();
        }
        public IActionResult AddToCart(int id)
        {
            loggedUser = this.userManager.GetUserId(HttpContext.User);
            if (loggedUser != null)
            {
                var productItem = Watches.GetProduct(id, ProductType.SmartWatches);
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

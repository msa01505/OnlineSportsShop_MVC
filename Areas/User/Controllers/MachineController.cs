using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Areas.User.Services;
using Proj.Areas.User.Services;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSportShop.Areas.User.Controllers
{     [Area("User")]
        
    public class MachineController : Controller
    {
        private readonly IProduct service;
        private readonly ICartList CartList;
        private readonly UserManager<Proj.Models.User> userManager;
        private string loggedUser;
        public MachineController(IProduct service, ICartList CartList,
            UserManager<Proj.Models.User> userManager
            )
        {
            this.userManager = userManager;
            this.CartList = CartList;
            this.service = service;
        }

        public IActionResult Index(int? id, string color, string name)
        {
            ViewData["ColorFilter"] = color;
            ViewData["NameFilter"] = name;

            List<Product> machineProductList = new List<Product>();

            if (color != null && name == null)
            {
                machineProductList = service.GetMachines().Where(l => l.Color.ToLower() == color.ToLower()).ToList();
            }
            else if (name != null && color == null)
            {
                machineProductList = service.GetMachines().Where(l => l.Name.ToString().ToLower() == name.ToLower()).ToList();
            }
            else if (color != null && name != null)
            {
                machineProductList = service.GetMachines().Where(
                    l => (l.Name.ToString().ToLower() == name.ToLower()) && (l.Color.ToLower() == color.ToLower())).ToList();
            }
            else
            {
                machineProductList = service.GetMachines();
            }
            ViewBag.machineProductList = machineProductList;

            return View();
        }

        //// GET: Admin/Clothes/Details/5
        public IActionResult Details(int id)
        {

            Product machine = service.GetProduct(id,ProductType.Machines);
            ViewBag.machine = machine;
            return View();

        }
        public IActionResult AddToCart(int id)
        {
            loggedUser = this.userManager.GetUserId(HttpContext.User);
            if (loggedUser != null)
            {
                var productItem = service.GetProduct(id, ProductType.Machines);
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

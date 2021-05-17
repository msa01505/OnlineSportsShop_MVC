using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Areas.User.Services;
using Proj.Areas.User.Services;
using Proj.Data;
using Proj.Models;
using System.Collections.Generic;

namespace OnlineSportShop.Areas.User.Controllers
{
    [Area("User")]
    public class CartListController : Controller
    {
        private readonly ICartList cartList;
        private readonly IProduct ProductList;
        private readonly UserManager<Proj.Models.User> userManager;
        private string loggedUser;
        public CartListController(ICartList cartList, IProduct ProductList, 
            UserManager<Proj.Models.User> userManager
            )
        {
            this.userManager = userManager;
            this.cartList = cartList;
            this.ProductList = ProductList;
            
            
        }

        public IActionResult Index()
        {
            loggedUser = this.userManager.GetUserId(HttpContext.User);
            if(loggedUser != null)
            {
                ShoppingCart shoppingCart = cartList.GetShoppingCart(loggedUser);
                List<ShoppingCartItem> items = cartList.GetShoppingCartItems(shoppingCart.ID);
                var Total = cartList.GetShoppingCartTotalPrice(shoppingCart.ID);
                ViewBag.ItemCount = items.Count;
                ViewBag.cart = items;
                ViewBag.Total = Total;

                return View();
            }
            else
            {
               
               return Redirect("https://localhost:44363/Identity/Account/Login");
                
            }
           
        }

        public IActionResult Success()
        {
            return View();
        }
        public IActionResult RemoveFromCart(int id)
        {
            
            cartList.RemoveItemFromCart(id);
            return RedirectToAction("Index");
        }
        public IActionResult Add(ShoppingCartItem Product, string userID)
        {
            var Type = Product.ProductItem.Type;
            var productItem = ProductList.GetProduct(Product.ProductItem.ID, Type);
            cartList.AddToCart(productItem, userID);
            return RedirectToAction("Index");
        }
    }
}

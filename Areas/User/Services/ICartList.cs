using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSportShop.Areas.User.Services
{
    public interface ICartList
    {
        public void CreatCartList(ShoppingCart shoppingCart);
        public ShoppingCart GetShoppingCart(string userID);
        public List<ShoppingCartItem> GetShoppingCartItems(int shoppingCartID);
        public void AddToCart(Product product, string UserID);
        public void RemoveItemFromCart(int itemID);
        public void ClearCart(int cartListID);
        public decimal GetShoppingCartTotalPrice(int cartListID);
    }
}

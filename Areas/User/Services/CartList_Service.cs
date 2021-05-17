using Microsoft.EntityFrameworkCore;
using Proj.Data;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineSportShop.Areas.User.Services
{
    public class CartList_Service : ICartList
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CartList_Service(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        //To create uniqe cart list for every user 
        public void CreatCartList(ShoppingCart shoppingCart)
        {
            applicationDbContext.shoppingCarts.Add(shoppingCart);
            applicationDbContext.SaveChanges();
        }
        public void AddToCart(Product product, string UserID)
        {
            var ShoppingCard = GetShoppingCart(UserID);
            var CartItems = GetShoppingCartItems(ShoppingCard.ID);
            if (IsExit(product,ShoppingCard.ID))
            {
                var Item = CartItems.FirstOrDefault(i => i.ProductItem.ID == product.ID);
                Item.TotalAmount++;
                applicationDbContext.shoppingCartItems.Update(Item);
                applicationDbContext.SaveChanges();
            }
            else
            {
            ShoppingCartItem shoppingCartItem = new  ShoppingCartItem
                { ShoppingCartID = ShoppingCard.ID,
                    ProductItem = product,
                    TotalAmount = 1};
                applicationDbContext.shoppingCartItems.Add(shoppingCartItem);
                applicationDbContext.SaveChanges();
            }
        }
        public bool IsExit(Product Product, int CartID)
        {
            var CartItems = GetShoppingCartItems(CartID);
            for(int i = 0; i < CartItems.Count(); i++)
            {
                if (CartItems[i].ProductItem.ID == Product.ID && 
                    CartItems[i].ProductItem.Type == Product.Type)
                    return true;
            }
            return false;
        }
        public void ClearCart(int cartListID)
        {
            var cartItems = applicationDbContext.shoppingCartItems.Where(cart => cart.ShoppingCartID == cartListID).ToList();

            applicationDbContext.shoppingCartItems.RemoveRange(cartItems);
            applicationDbContext.SaveChanges();
        }

        // to create new shopping cart

        public ShoppingCart GetShoppingCart(string userID)
        {
            ShoppingCart shoppingCart =
                applicationDbContext.shoppingCarts.FirstOrDefault(l => l.User_ID == userID);
            return shoppingCart;
        }

        public List<ShoppingCartItem> GetShoppingCartItems(int shoppingCartID)
        {
            List<ShoppingCartItem> cartItems = applicationDbContext.shoppingCartItems
                .Include(p => p.ProductItem)
                .Where(cart => cart.ShoppingCartID == shoppingCartID).ToList();
            return cartItems;

        }

        public decimal GetShoppingCartTotalPrice(int cartListID)
        {
            decimal totalPrice = 0;
            var CartItems = GetShoppingCartItems(cartListID);
            totalPrice = CartItems.Sum(c => c.ProductItem.Price * c.TotalAmount);
            return totalPrice;
        }

        public void RemoveItemFromCart(int itemID)
        {

            var res = applicationDbContext.shoppingCartItems.FirstOrDefault(item => item.ID == itemID);
            if(res.TotalAmount == 1) 
            {
                applicationDbContext.shoppingCartItems.Remove(res);
                applicationDbContext.SaveChanges();
            }
            else
            {
                res.TotalAmount--;
                applicationDbContext.shoppingCartItems.Update(res);
                applicationDbContext.SaveChanges();
            }
           
        }
    }
}

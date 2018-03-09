using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OnlneStore.Controllers
{
    public class OrderActionsController : ApiController
    {
        [System.Web.Mvc.ActionName("GetCartAmount")]
        public int GetCartAmount()
        {
            if (HttpContext.Current.User.Identity.Name == null)
                return 0;

            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            DataLayer dal = new DataLayer();
            List<CartItem> cartItems = (from x in dal.CartItems where x.UserId == userId select x).ToList<CartItem>();

            return (cartItems.Sum(x => x.Amount));
        }

        [Authorize]
        [System.Web.Mvc.ActionName("AddToCart")]
        public void AddToCart(Product pro)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new Exception("You need to login first!");
            DataLayer dal = new DataLayer();
            Product prod = (dal.Products.First(x => x.ProductId == pro.ProductId));

            CartItem item = new CartItem()
            {
                Amount = 1,
                Price = prod.Price,
                ProductId = prod.ProductId,
                UserId = int.Parse(HttpContext.Current.User.Identity.Name)
            };

            CartItem existingItem = (dal.CartItems.Find(item.ProductId, item.UserId));
            if (existingItem == null)
            {
                dal.CartItems.Add(item);
            }
            else
            {
                existingItem.Amount++;
            }
            dal.SaveChanges();  
        }

        [Authorize]
        public Object GetCartProducts()
        {
            DataLayer dal = new DataLayer();
            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            var list = (from x in dal.CartItems
                        join prod in dal.Products on x.ProductId equals prod.ProductId
                        where x.UserId == userId
                        select new
                        {
                            ProductId = prod.ProductId,
                            ProductName = prod.ProductName,
                            Price = x.Price,
                            Amount = x.Amount,
                            Total = x.Price * x.Amount
                        });
            return list;
        }

        [Authorize]
        public HttpResponseMessage MakeOrder()
        {
            DataLayer dal = new DataLayer();
            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            List<CartItem> cartItems = (from x in dal.CartItems
                                        where x.UserId == userId
                                        select x).ToList<CartItem>();

            Order order = new Order() {
                OrderTime = DateTime.Now,
                UserId = userId,
            };

            dal.Orders.Add(order);
            dal.SaveChanges();

            int orderId = order.OrderId;

            foreach (CartItem item in cartItems)
            {
                OrderItem oItem = new OrderItem()
                {
                    OrderId = orderId,
                    Price = item.Price,
                    Amount = item.Amount,
                    ProductId = item.ProductId
                };
                dal.OrderItems.Add(oItem);
                dal.CartItems.Remove(item);
            }
            dal.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Authorize]
        public HttpResponseMessage EmptyCart()
        {
            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            DataLayer dal = new DataLayer();

            List<CartItem> items = (from x in dal.CartItems
                                    where x.UserId == userId
                                    select x).ToList<CartItem>();

            foreach (CartItem item in items)
            {
                dal.CartItems.Remove(item);
            }
            dal.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Authorize]
        public HttpResponseMessage RemoveItemFromCart(CartItem item)
        {
            if (item != null)
            {
                int userId = int.Parse(HttpContext.Current.User.Identity.Name);
                DataLayer dal = new DataLayer();
                CartItem itemToRemove = dal.CartItems.Find(item.ProductId, userId);
                dal.CartItems.Remove(itemToRemove);
                dal.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

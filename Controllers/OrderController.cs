using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using OnlneStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlneStore.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShowOrders()
        {
            DataLayer dal = new DataLayer();
            OrdersView v = new OrdersView();
            int userId = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);

            v.Orders = (from x in dal.Orders
                         where x.UserId == userId
                         select new OrdersView
                         {
                             OrderId = x.OrderId,
                             OrderDate = x.OrderTime,
                             Amount = 0,
                             TotalPrice = 0
                         }).ToList<OrdersView>();

            foreach(OrdersView o in v.Orders)
            {
                List<OrderItem> items = (from x in dal.OrderItems where x.OrderId == o.OrderId select x).ToList<OrderItem>();
                o.Amount = items.Sum(x => x.Amount);
                o.TotalPrice = items.Sum(x => x.Amount * x.Price);
            }

            return View(v);
        }
    }
}
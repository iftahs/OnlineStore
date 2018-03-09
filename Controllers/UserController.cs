using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlneStore.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [Authorize]
        public ActionResult MyDetails()
        {
            DataLayer dal = new DataLayer();
            int userId = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);

            User user = dal.Users.Find(userId);
            return View(user);
        }
    }
}
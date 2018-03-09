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
    public class CategoryController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCategory(int id)
        {
            DataLayer dal = new DataLayer();
            CategoryView cav = new CategoryView();
            cav.category = (dal.Categories.First(x => x.CatId == id));
            cav.products = (from pro in dal.Products where pro.CatId == id select pro).ToList<Product>();

            return View(cav);
        }
    }
}
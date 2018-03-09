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
    public class ProductController : Controller
    {
        // GET: Product
        [Authorize(Roles ="admin")]
        public ActionResult AddProduct()
        {
            DataLayer dal = new DataLayer();
            CategoryView Cat = new CategoryView();
            Cat.categories = dal.Categories.ToList<Category>();
            return View(Cat);
        }

        public ActionResult ShowProduct(int id)
        {
            DataLayer dal = new DataLayer();
            Product product = dal.Products.Find(id);
            return View(product);
        }
    }
}
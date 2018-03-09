using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlneStore.Controllers
{
    public class CategoryActionsController : ApiController
    {
        public List<Category> GetAllCategories()
        {
            DataLayer dal = new DataLayer();
            List<Category> categories = (from cat in dal.Categories select cat).ToList<Category>();
            return categories;
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage AddCategory(Category cat)
        {
            if (cat.CatName != null)
            {
                DataLayer dal = new DataLayer();
                dal.Categories.Add(cat);
                dal.SaveChanges();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

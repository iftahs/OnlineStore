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
    public class ProductActionsController : ApiController
    {
        public string GetProductName(Product prod)
        {
            if (prod == null)
                return "";
            DataLayer dal = new DataLayer();
            Product product = (dal.Products.Find(prod.ProductId));
            if (product == null)
                return "Can't find product";
            return product.ProductName;
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage AddProduct(Product product)
        {
            DataLayer dal = new DataLayer();
            dal.Products.Add(product);
            dal.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

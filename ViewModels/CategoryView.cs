using OnlneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlneStore.ViewModels
{
    public class CategoryView
    {
        public Category category { get; set; }
        public List<Product> products { get; set; }
        public List<Category> categories { get; set; }
    }
}
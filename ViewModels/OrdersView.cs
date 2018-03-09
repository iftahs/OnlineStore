using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlneStore.ViewModels
{
    public class OrdersView
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int Amount { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrdersView> Orders { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlneStore.Models
{
    public class CartItem
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        public int UserId { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
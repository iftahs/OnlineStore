using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlneStore.Models
{
    public class Product
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(255)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("category")]
        public int CatId { get; set; }

        public Category category { get; set; }
    }
}
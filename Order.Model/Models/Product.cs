using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Order.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Order.Model.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string ShortDescription { get; set; }
        [AllowHtml]
        [MaxLength(300)]
        public string LongDescription { get; set; }
        public string MeasureUnit { get; set; }
        [Display(Name = "Netto Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Net Price")]
        public double NetPrice { get; set; }
        [Display(Name = "Brutto Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Net Price")]
        public double GrossPrice { get; set; }
        public bool ActiveState { get; set; }
        [Display(Name = "VAT Rate")]
        public VATRate VATRate { get; set; }

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        
        public int? MainImageId { get; set; }
        public Product()
        {
            ProductImages = new List<ProductImage>();
            MainImageId = 1;
        }   


    }
}

using Do_an_mon_hoc.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_mon_hoc.Dto.Products
{
    // Example: DTOs/ProductDto.cs
    public class ProductDto_Add
    {
        

        public string Thumbnail { get; set; }

        public string Name { get; set; }

        public int Reg_Price { get; set; }

        public int DiscountPercent { get; set; }

        public int DiscountPrice { get; set; }

        public string Unit { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Deleted { get; set; } 

        public double Rating { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }





    }

}

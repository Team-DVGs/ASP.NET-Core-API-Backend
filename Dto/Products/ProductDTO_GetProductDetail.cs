﻿using Do_an_mon_hoc.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_mon_hoc.Dto.Products
{
    // Example: DTOs/ProductDto.cs
    public class ProductDto_GetProductDetail
    {
        public int Id { get; set; }

        public string? thumbnail { get; set; }

        public string? Name { get; set; }

        public double? reg_price { get; set; }

        public int? discount_percent { get; set; }

        public double? discount_price { get; set; }

       public string canonical {  get; set; }

        public int quantity {  get; set; }
        public string? description { get; set; }

        //public int BrandId { get; set; }

        public double? rating { get; set; }

        public BrandDTO_GetIdName Brand { get; set; }

        

        //public int CategoryId { get; set; }
        //public string? category { get; set; }
        public CategoryDTO_GetIdName Category {  get; set; }

        public virtual ICollection<GalleryDTO_Get> galleries { get; set; } = new List<GalleryDTO_Get>();





    }

}

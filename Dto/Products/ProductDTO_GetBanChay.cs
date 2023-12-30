using Do_an_mon_hoc.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_mon_hoc.Dto.Products
{
    // Example: DTOs/ProductDto.cs
    public class ProductDto_GetBanChay
    {
        public String type { get; set; }

        public string query { get; set; }

        public IEnumerable<ProductDto_Get> products { get; set; }





    }

}

using Do_an_mon_hoc.Dto.Products;
using Do_an_mon_hoc.Models;

namespace Do_an_mon_hoc.Dto.CategoryGroup
{
    public class CategoryGroupDTO_GetBrands
    {
        public string name { get; set; } = null!;


        public virtual ICollection<BrandDTO_GetIdName> brands { get; set; } = new List<BrandDTO_GetIdName>();
    }
}

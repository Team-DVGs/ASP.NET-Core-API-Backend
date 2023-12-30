using Do_an_mon_hoc.Dto.Products;
using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CategoryDTO_GetProducts
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string description { get; set; }

    public string thumbnail { get; set; }

    public int product_quantity { get; set; }

    public virtual ICollection<ProductDto_Get> products { get; set; } = new List<ProductDto_Get>();






}

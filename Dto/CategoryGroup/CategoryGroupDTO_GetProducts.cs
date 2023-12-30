using Do_an_mon_hoc.Dto.Products;
using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CategoryGroupDTO_GetProducts
{
    public int categoryID { get; set; }

    public string name { get; set; } = null!;



    public string thumbnail { get; set; }


    public virtual ICollection<ProductDto_Get> products { get; set; } = new List<ProductDto_Get>();






}

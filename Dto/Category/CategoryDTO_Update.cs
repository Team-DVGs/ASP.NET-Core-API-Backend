using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CategoryDTO_Update
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; }

    public string Thumbnail { get; set; }

    public int ProductQuantity { get; set; }

    public int CategoryGroupId { get; set; }

    public virtual CategoryGroup? CategoryGroup { get; set; }

    public  ICollection<Product> Products { get; set; } = new List<Product>();
}

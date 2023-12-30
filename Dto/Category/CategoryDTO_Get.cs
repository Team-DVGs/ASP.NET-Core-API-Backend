using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CategoryDTO_Get
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string description { get; set; }

    public string thumbnail { get; set; }

    public int product_quantity { get; set; }

    public int category_group_id { get; set; }

    public string category_group_name {  get; set; }




}

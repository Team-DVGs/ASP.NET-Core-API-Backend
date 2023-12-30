using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CategoryGroupDTO_Update
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}

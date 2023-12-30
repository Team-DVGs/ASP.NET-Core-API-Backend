using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class GalleryDTO_Update
{
    public int Id { get; set; }

    public string? Thumbnail { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}

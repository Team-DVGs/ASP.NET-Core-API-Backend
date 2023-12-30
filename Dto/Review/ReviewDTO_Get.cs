using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class ReviewDTO_Get
{
    public int id { get; set; }

    public int? rating { get; set; }

    public string? title { get; set; }

    public string? comment { get; set; }

    public string? created_at { get; set; }

    //public int? ProductId { get; set; }

    //public int? UserId { get; set; }

   // public virtual Product? Product { get; set; }

    //public virtual User? User { get; set; }

    public string fullname {  get; set; }
}

using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class ReviewDTO_Add
{

    public int? rating { get; set; }

    public string? title { get; set; }

    public string? comment { get; set; }

    //public string? created_at { get; set; }

    public int? productId { get; set; }

    public int? userId { get; set; }


}

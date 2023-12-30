using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class ReviewDTO_Update
{
    public int? rating { get; set; }

    public string? title { get; set; }

    public string? comment { get; set; }

    public DateTime? created_at { get; set; }
}

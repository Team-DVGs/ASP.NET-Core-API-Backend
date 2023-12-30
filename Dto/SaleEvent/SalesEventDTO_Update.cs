using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class SalesEventDTO_Update
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual ICollection<SaleItem> Sales { get; set; } = new List<SaleItem>();
}

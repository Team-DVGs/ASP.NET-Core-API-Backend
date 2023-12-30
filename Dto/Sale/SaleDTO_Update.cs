﻿using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class SaleDTO_Update
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? EventId { get; set; }

    public int? Quantity { get; set; }

    public virtual SaleEvent? Event { get; set; }

    public virtual Product? Product { get; set; }
}

using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class OrderItemDTO_GetOrderItem
{
    public int Id { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    public double? Total { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}

using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class OrderDTO_Get
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public double? Total { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? payment_method { get; set; }

    public string? Date {  get; set; }

    //public DateTime? CreatedAt { get; set; }

    //public int? UserId { get; set; }

    public virtual ICollection<OrderItemDTO_Get> list { get; set; } = new List<OrderItemDTO_Get>();

    //public virtual User? User { get; set; }
}

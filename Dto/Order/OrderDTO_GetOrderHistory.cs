using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class OrderDTO_GetOrderHistory
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public double? Total { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

   // public string? PaymentMethod { get; set; }
   public string Thumbnail {  get; set; }
    public string? Date { get; set; }

    //public int? UserId { get; set; }

   // public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    //public virtual User? User { get; set; }
}

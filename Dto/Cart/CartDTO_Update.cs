using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class CartDTO_Update
{
    public int Id { get; set; }

    public int? Quantity { get; set; }

    public int? Total { get; set; }

    public int? Saved { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}

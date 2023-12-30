using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class UserDTO_Update
{
    //public int Id { get; set; }

    public string phone { get; set; } = null!;

    //public string? PasswordHash { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    //public DateTime? CreatedAt { get; set; }

    //public int? Deleted { get; set; }

    //public string Role { get; set; } = null!;

    //public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    //public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}

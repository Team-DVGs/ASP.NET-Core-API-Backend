﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Do_an_mon_hoc.Models;

public partial class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    //public override string ToString()
    //{
    //    return User.Id.ToString() + ": " + User.Fullname;
    //}
    public int? Quantity { get; set; }

    public double? Total { get; set; }

    public double? Saved { get; set; }

    public int? UserId { get; set; }

    [Display(AutoGenerateField = false)]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; }
}
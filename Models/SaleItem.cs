﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class SaleItem
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? EventId { get; set; }

    public int? Quantity { get; set; }

    public byte? IsDeleted { get; set; }

    public virtual SaleEvent Event { get; set; }

    public virtual Product Product { get; set; }
}
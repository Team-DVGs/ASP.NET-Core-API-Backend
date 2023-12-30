using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class SalesEventDTO_Get
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string description {  get; set; }

    //public int isOpen {  get; set; }

    public String start_time { get; set; }
    public String end_time { get; set; }
    //public DateTime? StartTime { get; set; }

    //public DateTime? EndTime { get; set; }

}

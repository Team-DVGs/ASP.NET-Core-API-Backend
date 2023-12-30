using System;
using System.Collections.Generic;

namespace Do_an_mon_hoc.Models;

public partial class FeedbackDTO_Update
{
    public int Id { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }
}

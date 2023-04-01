using System;
using System.Collections.Generic;

namespace MySuperApi.Models;

public partial class NewsApiSetting
{
    public int Id { get; set; }

    public string? Sourses { get; set; }

    public string? Domains { get; set; }

    public string? SearchTerm { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public string? Language { get; set; }

    public int? PageSize { get; set; }
}

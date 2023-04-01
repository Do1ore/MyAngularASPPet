using System;
using System.Collections.Generic;

namespace MySuperApi.Models;

public partial class ExtraImage
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public string? FileSource { get; set; }

    public int ProductId { get; set; }

    public int? ProductModelId { get; set; }

    public virtual Product? ProductModel { get; set; }
}

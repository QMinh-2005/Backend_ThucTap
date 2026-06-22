using System;
using System.Collections.Generic;

namespace Backend_ThucTap.Models;

public partial class ReturnRequestImage
{
    public int ImageId { get; set; }

    public int ReturnRequestId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ReturnRequest ReturnRequest { get; set; } = null!;
}

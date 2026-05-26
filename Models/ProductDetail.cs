using System;
using System.Collections.Generic;

namespace Backend_ThucTap.Models;

public partial class ProductDetail
{
    public int DetailId { get; set; }

    public int ProductId { get; set; }

    public string? WeightClass { get; set; }

    public string? GripSize { get; set; }

    public string? BalancePoint { get; set; }

    public string? Stiffness { get; set; }

    public int? MaxTension { get; set; }

    public decimal Price { get; set; }

    public int? StockQuantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}

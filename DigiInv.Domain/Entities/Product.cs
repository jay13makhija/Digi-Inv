namespace DigiInv.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? DigitalDownloadUrl { get; set; }
    
    // New Features
    public int StockQuantity { get; set; }
    public string Category { get; set; } = "General";
    public decimal DiscountAmount { get; set; }
}

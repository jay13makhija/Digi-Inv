namespace DigiInv.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? DigitalDownloadUrl { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? DigitalDownloadUrl { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = "General";
    public decimal DiscountAmount { get; set; }
}

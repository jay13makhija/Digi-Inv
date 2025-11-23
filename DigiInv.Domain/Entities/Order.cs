namespace DigiInv.Domain.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered, Cancelled
    public string ShippingAddress { get; set; } = string.Empty;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

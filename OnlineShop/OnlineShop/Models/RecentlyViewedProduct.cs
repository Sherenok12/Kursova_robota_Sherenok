namespace OnlineShop.Models;

public class RecentlyViewedProduct
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}

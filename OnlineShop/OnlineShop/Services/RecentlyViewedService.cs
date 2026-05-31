using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services;

public interface IRecentlyViewedService
{
    Task AddViewAsync(int userId, int productId);
    Task<List<Product>> GetRecentlyViewedAsync(int userId, int productIdToExclude, int count = 5);
}

public class RecentlyViewedService : IRecentlyViewedService
{
    private readonly AppDbContext _db;

    public RecentlyViewedService(AppDbContext db) => _db = db;

    public async Task AddViewAsync(int userId, int productId)
    {
        var existing = await _db.RecentlyViewedProducts
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

        if (existing != null)
        {
            existing.ViewedAt = DateTime.UtcNow;
        }
        else
        {
            _db.RecentlyViewedProducts.Add(new RecentlyViewedProduct
            {
                UserId = userId,
                ProductId = productId,
                ViewedAt = DateTime.UtcNow
            });
        }
        await _db.SaveChangesAsync();

        // Keep only last 10 records per user
        var old = await _db.RecentlyViewedProducts
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ViewedAt)
            .Skip(10)
            .ToListAsync();

        if (old.Any())
        {
            _db.RecentlyViewedProducts.RemoveRange(old);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> GetRecentlyViewedAsync(int userId, int productIdToExclude, int count = 5)
    {
        return await _db.RecentlyViewedProducts
            .Where(r => r.UserId == userId && r.ProductId != productIdToExclude)
            .OrderByDescending(r => r.ViewedAt)
            .Take(count)
            .Include(r => r.Product)
            .Select(r => r.Product!)
            .ToListAsync();
    }
}

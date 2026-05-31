using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services;

public interface ICartService
{
    Task<List<CartItem>> GetCartItemsAsync(int userId);
    Task AddToCartAsync(int userId, int productId);
    Task UpdateQuantityAsync(int cartItemId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(int userId);
    Task<int> GetCartCountAsync(int userId);
}

public class CartService : ICartService
{
    private readonly AppDbContext _db;

    public CartService(AppDbContext db) => _db = db;

    public async Task<List<CartItem>> GetCartItemsAsync(int userId)
    {
        return await _db.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task AddToCartAsync(int userId, int productId)
    {
        var existing = await _db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            _db.CartItems.Add(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1
            });
        }
        await _db.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int cartItemId, int quantity)
    {
        var item = await _db.CartItems.FindAsync(cartItemId);
        if (item != null)
        {
            if (quantity <= 0)
                _db.CartItems.Remove(item);
            else
                item.Quantity = quantity;
            await _db.SaveChangesAsync();
        }
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var item = await _db.CartItems.FindAsync(cartItemId);
        if (item != null)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int userId)
    {
        var items = await _db.CartItems.Where(c => c.UserId == userId).ToListAsync();
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    public async Task<int> GetCartCountAsync(int userId)
    {
        return await _db.CartItems.Where(c => c.UserId == userId).SumAsync(c => c.Quantity);
    }
}

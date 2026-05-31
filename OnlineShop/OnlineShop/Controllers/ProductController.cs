using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _db;
    private readonly IRecentlyViewedService _recentlyViewed;

    public ProductController(AppDbContext db, IRecentlyViewedService recentlyViewed)
    {
        _db = db;
        _recentlyViewed = recentlyViewed;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : null;
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        var comments = await _db.Comments
            .Include(c => c.User)
            .Where(c => c.ProductId == id)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var similar = await _db.Products
            .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
            .Take(4)
            .ToListAsync();

        var recentlyViewed = new List<Models.Product>();
        var userId = GetUserId();
        if (userId.HasValue)
        {
            await _recentlyViewed.AddViewAsync(userId.Value, id);
            recentlyViewed = await _recentlyViewed.GetRecentlyViewedAsync(userId.Value, id);
        }

        var vm = new ProductDetailViewModel
        {
            Product = product,
            Comments = comments,
            SimilarProducts = similar,
            RecentlyViewed = recentlyViewed
        };

        return View(vm);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.ShortDescription.Contains(search));

        var products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        var categories = await _db.Categories.ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.SearchQuery = search;

        return View(products);
    }
}

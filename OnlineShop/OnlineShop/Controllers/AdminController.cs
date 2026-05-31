using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    // ===== Products =====

    public async Task<IActionResult> Products()
    {
        var products = await _db.Products.Include(p => p.Category).OrderByDescending(p => p.CreatedAt).ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        var vm = new ProductFormViewModel { Categories = await _db.Categories.ToListAsync() };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductFormViewModel model)
    {
        model.Categories = await _db.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);

        var product = new Product
        {
            Name = model.Name,
            ShortDescription = model.ShortDescription,
            FullDescription = model.FullDescription,
            Price = model.Price,
            ImagePath = model.ImagePath,
            CategoryId = model.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Товар створено!";
        return RedirectToAction("Products");
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        var vm = new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            ShortDescription = product.ShortDescription,
            FullDescription = product.FullDescription,
            Price = product.Price,
            ImagePath = product.ImagePath,
            CategoryId = product.CategoryId,
            Categories = await _db.Categories.ToListAsync()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(ProductFormViewModel model)
    {
        model.Categories = await _db.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);

        var product = await _db.Products.FindAsync(model.Id);
        if (product == null) return NotFound();

        product.Name = model.Name;
        product.ShortDescription = model.ShortDescription;
        product.FullDescription = model.FullDescription;
        product.Price = model.Price;
        product.ImagePath = model.ImagePath;
        product.CategoryId = model.CategoryId;

        await _db.SaveChangesAsync();
        TempData["Message"] = "Товар оновлено!";
        return RedirectToAction("Products");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Message"] = "Товар видалено!";
        }
        return RedirectToAction("Products");
    }

    // ===== Categories =====

    public async Task<IActionResult> Categories()
    {
        var categories = await _db.Categories.Include(c => c.Products).ToListAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult CreateCategory() => View();

    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Categories.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Категорію створено!";
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Category model)
    {
        if (!ModelState.IsValid) return View(model);

        var category = await _db.Categories.FindAsync(model.Id);
        if (category == null) return NotFound();

        category.Name = model.Name;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Категорію оновлено!";
        return RedirectToAction("Categories");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            if (category.Products.Any())
            {
                TempData["Error"] = "Неможливо видалити категорію з товарами!";
                return RedirectToAction("Categories");
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData["Message"] = "Категорію видалено!";
        }
        return RedirectToAction("Categories");
    }
}

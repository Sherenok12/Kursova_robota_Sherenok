using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly AppDbContext _db;

    public CommentController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Add(int productId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            TempData["Error"] = "Коментар не може бути порожнім";
            return RedirectToAction("Details", "Product", new { id = productId });
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        _db.Comments.Add(new Comment
        {
            ProductId = productId,
            UserId = userId,
            Text = text.Trim(),
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        return RedirectToAction("Details", "Product", new { id = productId });
    }
}

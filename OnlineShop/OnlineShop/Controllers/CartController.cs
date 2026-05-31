using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cart;

    public CartController(ICartService cart) => _cart = cart;

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public async Task<IActionResult> Index()
    {
        var items = await _cart.GetCartItemsAsync(GetUserId());
        return View(new CartViewModel { Items = items });
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, string? returnUrl)
    {
        await _cart.AddToCartAsync(GetUserId(), productId);
        TempData["Message"] = "Товар додано в кошик!";
        return Redirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        await _cart.UpdateQuantityAsync(cartItemId, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        await _cart.RemoveFromCartAsync(cartItemId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        await _cart.ClearCartAsync(GetUserId());
        return RedirectToAction("Index");
    }
}

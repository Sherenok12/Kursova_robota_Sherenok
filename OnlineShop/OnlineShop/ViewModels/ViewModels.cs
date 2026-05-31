using System.ComponentModel.DataAnnotations;
using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введіть email")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введіть ім'я користувача")]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть email")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть пароль")]
    [MinLength(6, ErrorMessage = "Пароль має містити мінімум 6 символів")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердіть пароль")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ProductDetailViewModel
{
    public Product Product { get; set; } = null!;
    public List<Comment> Comments { get; set; } = new();
    public List<Product> SimilarProducts { get; set; } = new();
    public List<Product> RecentlyViewed { get; set; } = new();
}

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
}

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть назву")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string ShortDescription { get; set; } = string.Empty;

    public string FullDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть ціну")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більше 0")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string ImagePath { get; set; } = string.Empty;

    [Required(ErrorMessage = "Оберіть категорію")]
    public int CategoryId { get; set; }

    public List<Category> Categories { get; set; } = new();
}

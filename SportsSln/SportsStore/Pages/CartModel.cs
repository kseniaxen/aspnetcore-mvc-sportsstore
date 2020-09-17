using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Pages
{
  public class CartModel : PageModel
  {
    private IStoreRepository repository;
    public CartModel(IStoreRepository repo)
    {
      repository = repo;
    }
    public Cart Cart { get; set; }
    public string ReturnUrl { get; set; }
    public void OnGet(string returnUrl)
    {
      ReturnUrl = returnUrl ?? "/";
      // читаем из http-сеанса модель корзины,
      // но если пользователь не положил в корзину еще ни одного товара -
      // создаем впервые пустой экземпляр модели корзины
      Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
    }
    public IActionResult OnPost(long productId, string returnUrl)
    {
      // когда пользователь добавляет в корзину товар
      Product product = repository.Products
        .FirstOrDefault(p => p.ProductID == productId);
      Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
      // увеличиваем его количество в модели корзины
      Cart.AddItem(product, 1);
      // и сохраняем модель обратно в сеанс
      HttpContext.Session.SetJson("cart", Cart);
      // возвращаем команду перенаправления
      return RedirectToPage(new { returnUrl = returnUrl });
    }
  }
}
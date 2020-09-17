using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
  public class SessionCart : Cart
  {
    // получение ссылки на предоставитель службы
    // внедрением
    public static Cart GetCart(IServiceProvider services)
    {
      // попытка получить от предоставителя ссылку на службу,
      // и из последней - на сессию
      ISession session =
        services.GetRequiredService<IHttpContextAccessor>()?
          .HttpContext.Session;
          // если удалось - извлекается корзина из сессии
      SessionCart cart = session?.GetJson<SessionCart>("Cart")
      ?? new SessionCart();
      // иначе - создается впервые
      cart.Session = session;
      return cart;
    }
    // так как данный класс - это, помимо реализации логики, еще и модель,
    // экземпляры которой будут сериализоваться при сохранении в сеанс,
    // нужно все вспомогательные члены класса, и те,
    // которые могут вызвать бесконечный рекурсивный цикл сериализации,
    // пометить атрибутом,
    // защищающим от сериализации 
    [JsonIgnore]
    public ISession Session { get; set; }
    public override void AddItem(Product product, int quantity)
    {
      base.AddItem(product, quantity);
      Session.SetJson("Cart", this);
    }
    public override void RemoveLine(Product product)
    {
      base.RemoveLine(product);
      Session.SetJson("Cart", this);
    }
    public override void Clear()
    {
      base.Clear();
      Session.Remove("Cart");
    }
  }
}
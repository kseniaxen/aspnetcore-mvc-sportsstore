using System.Collections.Generic;
using System.Linq;
namespace SportsStore.Models
{
  public class Cart
  {
    // полный список товаров в корзине
    public List<CartLine> Lines { get; set; } = new List<CartLine>();
    public void AddItem(Product product, int quantity)
    {
      // добавлял ли пользователь хотя бы один такой товар в корзину?
      CartLine line = Lines
        .Where(p => p.Product.ProductID == product.ProductID)
        .FirstOrDefault();
      // если нет - формируем модель пункта корзины и добавляем
      // в список
      if (line == null)
      {
        Lines.Add(new CartLine
        {
          Product = product,
          Quantity = quantity
        });
      }
      // иначе если такой товар уже встречается в корзине -
      // увеличиваем его количество в модели пункта корзины
      else
      {
        line.Quantity += quantity;
      }
    }
    public void RemoveLine(Product product) =>
    Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);
    public decimal ComputeTotalValue() =>
    Lines.Sum(e => e.Product.Price * e.Quantity);
    public void Clear() => Lines.Clear();
  }
  // модель одного пункта в корзине покупок:
  // товар - количество
  public class CartLine
  {
    public int CartLineID { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
  }
}
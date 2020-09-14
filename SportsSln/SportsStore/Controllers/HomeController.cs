using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers {

    public class HomeController: Controller {

        // поле для хранения ссылки на репозиторий
        private IStoreRepository repository;

        // внедрение зависимости через аргумент конструктора
        public HomeController(IStoreRepository repo) {
            repository = repo;
        }

        // вывод основной страницы с передачей ей в модель всего списка описаний товаров
        public ViewResult Index() => View(repository.Products);
    }

    // public IActionResult ProductsLit1() => (new StoreDbContext()).Products;
    // public IActionResult ProductsLit2() => (new StoreDbContext()).Products.ToList();
}
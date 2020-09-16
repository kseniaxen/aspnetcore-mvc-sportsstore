using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
// using System;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers {

    public class HomeController : Controller {

        // поле для хранения ссылки на репозиторий
        private IStoreRepository repository;
        // товаров на одной странице
        public int PageSize = 4;

        // внедрение зависимости через аргумент конструктора
        public HomeController (IStoreRepository repo) {
            repository = repo;
        }

        // вывод основной страницы с передачей ей в модель всего списка описаний товаров
        /* public ViewResult Index() {
            // this.ViewBag.PaginationInfo = "some data";
            // this.ViewBag.Data = 101;
            return View(repository.Products);
        } */

        // home/index/2
        public ViewResult Index (string category, int productPage = 1) => View (new ProductsListViewModel {
            Products = repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy (p => p.ProductID)
                .Skip ((productPage - 1) * PageSize)
                .Take (PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.Products.Count ()
                }
        });

    }

    // public IActionResult ProductsLit1() => (new StoreDbContext()).Products;
    // public IActionResult ProductsLit2() => (new StoreDbContext()).Products.ToList();
}
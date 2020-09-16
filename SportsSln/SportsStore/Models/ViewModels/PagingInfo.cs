using System;
namespace SportsStore.Models.ViewModels {
    /* Модель для части представления Товары: гиперссылки постраничности */
    public class PagingInfo {
        // всего товаров
        public int TotalItems { get; set; }
        // максимум товаров на одну стрницу
        public int ItemsPerPage { get; set; }
        // номер текущей страницы
        public int CurrentPage { get; set; }
        // вычисляемое свойство - всего страниц
        public int TotalPages =>
            (int) Math.Ceiling ((decimal) TotalItems / ItemsPerPage);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Xunit;
namespace SportsStore.Tests {
    public class PageLinkTagHelperTests {
        [Fact]
        public void Can_Generate_Page_Links () {
            // Arrange
            // макет построителя гиперссылок
            var urlHelper = new Mock<IUrlHelper> ();
            // когда у построителя гиперссылок
            // будет вызван како-либо метод, типизированный UrlActionContext,
            urlHelper.SetupSequence (x =>
                    x.Action (It.IsAny<UrlActionContext> ()))
                    // - вернуть список строк с адресами
                .Returns ("Test/Page1")
                .Returns ("Test/Page2")
                .Returns ("Test/Page3");
            // настройка макета фабрики генераторов адресов на выдачу
            // выше определенного макета с заданным ему списком адресов
            var urlHelperFactory = new Mock<IUrlHelperFactory> ();
            urlHelperFactory.Setup (f =>
                    f.GetUrlHelper (It.IsAny<ActionContext> ()))
                .Returns (urlHelper.Object);
            // создаем экземпляр реализации тестируемого класса,
            // передавая ему вручную ссылку на макет фабрики построителей гиперссылок
            PageLinkTagHelper helper =
                new PageLinkTagHelper (urlHelperFactory.Object) {
                    PageModel = new PagingInfo {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                    },
                    PageAction = "Test"
                };
            // создание пустого объекта контекста для помощников
            TagHelperContext ctx = new TagHelperContext (
                new TagHelperAttributeList (),
                new Dictionary<object, object> (), "");
            // макет объекта содержимого формируемой модели узла разметки
            var content = new Mock<TagHelperContent> ();
            TagHelperOutput output = new TagHelperOutput ("div",
                new TagHelperAttributeList (),
                (cache, encoder) => Task.FromResult (content.Object));
            // Act
            // вызываем вручную метод, который в приложении будет вызываться у помощника
            // представлением, в котором используется помощник
            helper.Process (ctx, output);
            // Assert
            // Проверка сгенерированного фрагмента разметки на соответствие заданной модели
            Assert.Equal (@"<a href=""Test/Page1"">1</a>" +
                @"<a href=""Test/Page2"">2</a>" +
                @"<a href=""Test/Page3"">3</a>",
                output.Content.GetContent ());
        }
    }
}
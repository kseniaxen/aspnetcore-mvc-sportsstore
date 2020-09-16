using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;
namespace SportsStore.Infrastructure {

    /* Построитель части представления,
    основанный на стандартном помощнике TagHelper */
    [HtmlTargetElement ("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper {

        private IUrlHelperFactory urlHelperFactory;
        // внедрение стандартной фабрики построения веб-адресов
        public PageLinkTagHelper (IUrlHelperFactory helperFactory) {
            urlHelperFactory = helperFactory;
        }

        // внедрение контекста представления
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process (TagHelperContext context,
            TagHelperOutput output) {
            IUrlHelper urlHelper =
                urlHelperFactory.GetUrlHelper (ViewContext);
            // создание С#-модели узла div
            TagBuilder result = new TagBuilder ("div");
            // заполнение корневого узла дочерними - гиперссылками, ведущими на страницы товаров
            for (int i = 1; i <= PageModel.TotalPages; i++) {
                // // создание С#-модели узла гиперссылки
                TagBuilder tag = new TagBuilder ("a");
                // настройка адреса гиперссылки
                tag.Attributes["href"] = urlHelper.Action (PageAction,
                    new { productPage = i });
                if (PageClassesEnabled) {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                    ? PageClassSelected : PageClassNormal);
                }
                // настройка отображаемого текста каждой гиперссылки постраничности
                tag.InnerHtml.Append (i.ToString ());
                // добавление каждой гиперссылки постраничности в модель узла div
                result.InnerHtml.AppendHtml (tag);
            }
            // выдача готового фрагмента разметки - блока с гиперссылками постраничности
            output.Content.AppendHtml (result.InnerHtml);
        }
    }
}
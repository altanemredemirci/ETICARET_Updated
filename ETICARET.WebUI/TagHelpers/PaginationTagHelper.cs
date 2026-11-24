using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ETICARET.WebUI.TagHelpers
{
    [HtmlTargetElement("pagination",Attributes ="page-info")]
    public class PaginationTagHelper(IUrlHelperFactory urlHelperFactory):TagHelper
    {
        //Razor bu özelliği mevcut sayfanın bağlamıyla otomatik doldurur.
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default;

        public PageInfo PageInfo { get; set; } = default;

        public string PageAction { get; set; } = "List";

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url")]
        public Dictionary<string, object?> PageUrlValues { get; set; } = [];

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageInfo.TotalPages <= 1)
            {
                output.SuppressOutput();
                return;
            }

            var url = urlHelperFactory.GetUrlHelper(ViewContext);

            var list = new TagBuilder("ul"); //<ul class="pagination justify-content-center mb-0">
                
            list.AddCssClass("pagination justify-content-center mb-0");

            list.InnerHtml.AppendHtml(PageItem(url, PageInfo.CurrentPage - 1, "< Önceki", disabled: !PageInfo.HasPrevious));

            for (var page = 1; page <= PageInfo.TotalPages; page++)
                list.InnerHtml.AppendHtml(PageItem(url, page, page.ToString(), active: page == PageInfo.CurrentPage));
            list.InnerHtml.AppendHtml(PageItem(url, PageInfo.CurrentPage + 1, "Sonraki ›", disabled: !PageInfo.HasNext));

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("aria-label", "Sayfalar");
            output.Content.SetHtmlContent(list);
        }

        private TagBuilder PageItem(IUrlHelper url, int page, string text, bool active = false, bool disabled = false)
        {
            var item = new TagBuilder("li");
            item.AddCssClass("page-item");
            if (active) item.AddCssClass("active");
            if (disabled) item.AddCssClass("disabled");

            var link = new TagBuilder("a");
            link.AddCssClass("page-link");
            PageUrlValues["page"] = page;
            link.Attributes["href"] = url.Action(PageAction, PageUrlValues);
            link.InnerHtml.Append(text);

            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}

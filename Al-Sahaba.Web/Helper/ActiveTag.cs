using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Al_Sahaba.Web.Helper
{
    [HtmlTargetElement("a", Attributes = "active")]
    public class ActiveTag : TagHelper
    {
        public string? Active { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Active))
            {
                return;
            }
            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();
            var currentAction = ViewContextData?.RouteData.Values["action"]?.ToString();


            if (currentController == Active || currentAction == Active)
            {

                if (output.Attributes.ContainsName("class"))
                {
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }
    }
}

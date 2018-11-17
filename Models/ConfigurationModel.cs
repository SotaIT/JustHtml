using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.JustHtml.Models
{
    public class ConfigurationModel : BaseSearchModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Admin.JustHtml.WidgetSeparator")]
        public string WidgetSeparator { get; set; }

        public bool WidgetSeparator_OverrideForStore { get; set; }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.JustHtml.Models
{
    public class WidgetModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public int Id { get; set; }

        public bool Id_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.JustHtml.Widgets.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.JustHtml.Widgets.Fields.Content")]
        public string Content { get; set; }

        [NopResourceDisplayName("Admin.JustHtml.Widgets.Fields.WidgetZone")]
        public string WidgetZone { get; set; }

        [NopResourceDisplayName("Admin.JustHtml.Widgets.Fields.Order")]
        public int Order { get; set; }

        public IList<SelectListItem> AvailableWidgetZones { get; set; }

    }
}
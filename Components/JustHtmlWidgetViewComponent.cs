using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.JustHtml.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.JustHtml.Components
{
    [ViewComponent(Name = JustHtmlWidget)]
    public class JustHtmlWidgetViewComponent : NopViewComponent
    {
        public const string JustHtmlWidget = "JustHtmlWidget";
        private readonly IJustHtmlWidgetService _widgetService;
        private readonly JustHtmlSettings _settings;
        private readonly int _storeScope;

        public JustHtmlWidgetViewComponent(IStoreContext storeContext,
            IJustHtmlWidgetService widgetService,
            ISettingService settingService)
        {
            _widgetService = widgetService;

            _settings = settingService.LoadSetting<JustHtmlSettings>();
            _storeScope = storeContext.ActiveStoreScopeConfiguration;

        }
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var widgetContents = _widgetService.GetAll()
                .Where(w => w.IsActive
                            && string.Equals(w.WidgetZone, widgetZone, StringComparison.OrdinalIgnoreCase)
                            && (w.StoreId == 0 || w.StoreId == _storeScope))
                .OrderBy(w => w.Order)
                .Select(w => w.Content);

            return View("~/Plugins/Widgets.JustHtml/Views/PublicInfo.cshtml", string.Join(_settings.WidgetSeparator, widgetContents));
        }
    }
}
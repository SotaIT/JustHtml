using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Plugin.Widgets.JustHtml.Components;
using Nop.Plugin.Widgets.JustHtml.Data;
using Nop.Plugin.Widgets.JustHtml.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Widgets.JustHtml
{
    public class JustHtmlPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IJustHtmlWidgetService _widgetService;
        private readonly ISettingService _settingService;
        private readonly WidgetObjectContext _context;
        private readonly IWebHelper _webHelper;
        private readonly int _storeScope;

        public JustHtmlPlugin(ILocalizationService localizationService,
            IStoreContext storeContext,
            IJustHtmlWidgetService widgetService,
            ISettingService settingService,
            IWebHelper webHelper
            , WidgetObjectContext context)
        {
            _localizationService = localizationService;
            _widgetService = widgetService;
            _settingService = settingService;
            _context = context;
            _webHelper = webHelper;

            _storeScope = storeContext.ActiveStoreScopeConfiguration;
        }

        public bool HideInWidgetList => false;

        public IList<string> GetWidgetZones()
        {
            return _widgetService.GetAll()
                .Where(w => w.IsActive && (w.StoreId == 0 || w.StoreId == _storeScope))
                .Select(w => w.WidgetZone)
                .Distinct()
                .ToList();
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return JustHtmlWidgetViewComponent.JustHtmlWidget;
        }
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/JustHtmlWidget/Configure";
        }

        public override void Install()
        {
            //settings init
            var settings = new JustHtmlSettings
            {
                WidgetSeparator = string.Empty
            };

            //settings
            _settingService.SaveSetting(settings);

            //database
            _context.Install();

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.WidgetSeparator", "Widget Separator");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Instructions", "Configure settings");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets", "Widgets");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Description", "Widget list");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Instructions", "Edit widget properties");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Fields.IsActive", "Is Active");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Fields.Content", "Content");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Fields.WidgetZone", "WidgetZone");
            _localizationService.AddOrUpdatePluginLocaleResource("Admin.JustHtml.Widgets.Fields.Order", "Order");

            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<JustHtmlSettings>();

            //database
            _context.Uninstall();

            //locales
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.WidgetSeparator");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Instructions");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Description");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Instructions");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Fields.IsActive");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Fields.Content");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Fields.WidgetZone");
            _localizationService.DeletePluginLocaleResource("Admin.JustHtml.Widgets.Fields.Order");

            base.Uninstall();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Widgets.JustHtml.Data.Domain;
using Nop.Plugin.Widgets.JustHtml.Models;
using Nop.Plugin.Widgets.JustHtml.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.JustHtml.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AdminAntiForgery]
    public class JustHtmlWidgetController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IJustHtmlWidgetService _widgetService;
        private readonly ISettingService _settingService;

        private readonly JustHtmlSettings _settings;
        private readonly int _storeScope;

        #endregion

        #region Ctor

        public JustHtmlWidgetController(IStoreContext storeContext,
            ISettingService settingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IJustHtmlWidgetService widgetService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _widgetService = widgetService;

            _storeScope = storeContext.ActiveStoreScopeConfiguration;
            _settings = settingService.LoadSetting<JustHtmlSettings>(_storeScope);

        }

        #endregion

        #region Utils

        private IList<SelectListItem> GetWidgetZones()
        {
            return typeof(PublicWidgetZones)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.CanRead && p.PropertyType == typeof(string))
                .Select(p => new SelectListItem($"{p.Name} ({p.GetValue(null)})", p.GetValue(null).ToString()))
                .ToList();
        }

        private string GetWidgetZoneName(string value)
        {
            return typeof(PublicWidgetZones)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(p => p.CanRead
                                     && p.PropertyType == typeof(string)
                                     && string.Equals(p.GetValue(null).ToString(), value, StringComparison.OrdinalIgnoreCase))
                ?.Name;
        }

        #endregion

        #region Methods

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ActiveStoreScopeConfiguration = _storeScope,
                WidgetSeparator = _settings.WidgetSeparator,
                WidgetSeparator_OverrideForStore =
                    _settingService.SettingExists(_settings, x => x.WidgetSeparator, _storeScope)
            };

            // ReSharper disable once Mvc.ViewNotResolved
            return View("~/Plugins/Widgets.JustHtml/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            _settings.WidgetSeparator = model.WidgetSeparator;
            _settingService.SaveSettingOverridablePerStore(_settings, x => x.WidgetSeparator,
                model.WidgetSeparator_OverrideForStore, _storeScope, false);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            var widgets = _widgetService
                .GetAll()
                .Where(w => w.StoreId == 0 || w.StoreId == _storeScope)
                .OrderBy(w => w.Order)
                .Select(w => new WidgetModel
                {
                    Id = w.Id,
                    IsActive = w.IsActive,
                    Content = w.Content,
                    Order = w.Order,
                    WidgetZone = $"{GetWidgetZoneName(w.WidgetZone)} ({w.WidgetZone})"
                })
                .ToList();
            var model = new WidgetListModel
            {
                Data = widgets,
                Total = widgets.Count
            };

            return Json(model);

        }

        public IActionResult ConfigureItem(int id = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var model = new WidgetModel
            {
                ActiveStoreScopeConfiguration = _storeScope,
                AvailableWidgetZones = GetWidgetZones()
            };
            if (id != 0)
            {
                var widget = _widgetService.GetById(id);
                if (widget != null)
                {
                    model.Id = id;
                    model.Content = widget.Content;
                    model.IsActive = widget.IsActive;
                    model.Order = widget.Order;
                    model.WidgetZone = widget.WidgetZone;
                }
                else
                {
                    return NotFound();
                }
            }

            // ReSharper disable once Mvc.ViewNotResolved
            return View("~/Plugins/Widgets.JustHtml/Views/ConfigureItem.cshtml", model);
        }

        [HttpPost]
        public IActionResult ConfigureItem(WidgetModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var widget = model.Id == 0 ? new JustHtmlWidget() : _widgetService.GetById(model.Id);

            if (widget == null)
            {
                return NotFound();
            }

            widget.WidgetZone = model.WidgetZone;
            widget.Content = model.Content;
            widget.IsActive = model.IsActive;
            widget.Order = model.Order;

            if (model.Id == 0)
            {
                _widgetService.InsertWidget(widget);
            }
            else
            {
                _widgetService.UpdateWidget(widget);
            }

            //todo: success message and add return to list
            //SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            //return ConfigureItem();

            return RedirectToAction(nameof(Configure));
        }

        public IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var widget = _widgetService.GetById(id);
            if (widget != null)
            {
                _widgetService.DeleteWidget(widget);
            }

            return RedirectToAction(nameof(Configure));
        }

        #endregion
    }
}
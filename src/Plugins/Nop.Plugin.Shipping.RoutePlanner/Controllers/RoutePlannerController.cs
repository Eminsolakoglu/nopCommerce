using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Plugin.Shipping.RoutePlanner.Models;
using Nop.Plugin.Shipping.RoutePlanner.Services;

using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.RoutePlanner.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class RoutePlannerController :BasePluginController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRoutePlannerService _routePlannerService;

        #endregion

        #region Ctor
        public RoutePlannerController(
            IPermissionService permissionService,
            IStoreContext storeContext,
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IRepository<Address> addressRepository,
            IRepository<Order> orderRepository,
            IRoutePlannerService routePlannerService // Add this


            )
        {
            _permissionService= permissionService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _routePlannerService = routePlannerService;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> OrderRouting()
        {
            // Modeli oluşturup gerekli verileri atıyoruz
            var ordersWithCounties =await _routePlannerService.GetOrdersNotPickedUpWithCounties();
            var counties = _routePlannerService.GetCounties(); // GetCounties çağrısı
            var model = new ConfigurationModel
            {
                OrdersNotPickedUp = ordersWithCounties,
                AvailableCounties = counties.Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                }).ToList()
            };

            return View("~/Plugins/Shipping.RoutePlanner/Views/OrderRouting.cshtml", model);
        }
        [HttpPost]
        public IActionResult OrderRouting(ConfigurationModel model)
        {
            var selectedCounties = model.SelectedCounties;

            // Seçilen ilçelerle ilgili işlemler burada yapılır

            return RedirectToAction("OrderRouting");
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var storeScope=await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<RoutePlannerSettings>(storeScope);
            var ordersNotPickedUp = _routePlannerService.GetOrdersNotPickedUp();
            var counties = _routePlannerService.GetCounties(); // GetCounties çağrısı
            var ordersWithCounties = await _routePlannerService.GetOrdersNotPickedUpWithCounties();




            var model = new ConfigurationModel
            {
                WidgetzoneContent = settings.WidgetzoneContent,
                WidgetzoneContent_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.WidgetzoneContent, storeScope),
                OrderCount = _routePlannerService.GetGreenaddressCount(),

                OrdersNotPickedUp = ordersWithCounties // Güncellenmiş veri



            };
            return View(RoutePlannerDefaults.ViewPath + "Configure.cshtml",model);
        }
        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<RoutePlannerSettings>(storeScope);

            settings.WidgetzoneContent = model.WidgetzoneContent;
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.WidgetzoneContent, model.WidgetzoneContent_OverrideForStore, storeScope);

            //Clear settings cache
            await _settingService.ClearCacheAsync();
            //Notifiction
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            return RedirectToAction("Configure");
        }

        #endregion
    }
}

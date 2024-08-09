using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Plugin.Shipping.RoutePlanner.Models;
using Nop.Web.Framework.Components;
using Nop.Services.Configuration;
using Nop.Plugin.Shipping.RoutePlanner.Services;


namespace Nop.Plugin.Shipping.RoutePlanner.Components
{
    
    public class RoutePlannerComponent: NopViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IRoutePlannerService _routePlannerService;

        public RoutePlannerComponent(
            ISettingService settingService,
            IRoutePlannerService routePlannerService
            )
        {
            _settingService = settingService;
            _routePlannerService = routePlannerService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object addtionalData)
        {
            var settings = await _settingService.LoadSettingAsync<RoutePlannerSettings>();

            var orderCount = await _routePlannerService.GetOrderCount();

            var model = new RoutePlannerHeaderModelcs
            {
                WidgetContent = settings.WidgetzoneContent,
                OrderCount = orderCount,
            };
            return View(RoutePlannerDefaults.ViewPath+ "RoutePlannerWidgetZone.cshtml",model);
        }
    }
}

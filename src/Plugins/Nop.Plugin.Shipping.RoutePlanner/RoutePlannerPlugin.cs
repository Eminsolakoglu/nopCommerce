using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Shipping.RoutePlanner
{
    public class RoutePlannerPlugin : BasePlugin, IWidgetPlugin ,IAdminMenuPlugin
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWebHelper _webHelper;

        #endregion
        #region Ctor
        public RoutePlannerPlugin(
            ISettingService settingService,
            ILocalizationService localizationService,
            ILanguageService languageService,
            IWebHelper webHelper
            )
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _languageService = languageService;
            _webHelper = webHelper;
        }


        #endregion


        #region Methods
        /// <summary>
        /// gets a configuration pg url
        /// </summary>
        /// <returns></returns>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/RoutePlanner/Configure";
        }
        /// <summary>
        /// ınstall th pulgin
        /// </summary>
        /// <returns></returns>
        public override async Task InstallAsync()
        {   //settings
            var settings = new RoutePlannerSettings
            {
                WidgetzoneContent = "",
            };
            await _settingService.SaveSettingAsync(settings);



            #region Locales


            var langs =await _languageService.GetAllLanguagesAsync();
            var trLang= langs.FirstOrDefault(l=>l.LanguageCulture=="tr-TR");
            if (trLang != null)
            {
                await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
                {
                    ["Plugins.Shipping.RoutePlanner.Configuration.WidgetzoneContent"] = "Widget Zone İçeriği",
                    ["Plugins.Shipping.RoutePlanner.Configuration.WidgetzoneContent.Hint"] = "Lütfen WidgetZone içeriği giriniz"
                }, trLang.Id);
            }
            var enLang = langs.FirstOrDefault(l => l.LanguageCulture == "en-US");
            if (enLang != null)
            {
                await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
                {
                    ["Plugins.Shipping.RoutePlanner.Configuration.WidgetzoneContent"] = "Widget Zone content",
                    ["Plugins.Shipping.RoutePlanner.Configuration.WidgetzoneContent.Hint"] = "Lütfen WidgetZonecontent enter"
                }, enLang.Id);
            }
            #endregion

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizationService.DeleteLocaleResourceAsync("Plugins.Shipping.RoutePlanner");

            await base.UninstallAsync();
        }



        #endregion
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            if(widgetZone == null)
            {
                throw new ArgumentException(null,nameof(widgetZone));
            }
            return RoutePlannerDefaults.RoutePlannerViewComponentName;
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            var widgetZones = new List<string>
            {
                PublicWidgetZones.HeaderMiddle
            };
            return await Task.FromResult(widgetZones);
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
                var menuItem = new SiteMapNode()
                {
                    SystemName = "Shipping.RoutePlanner",
                    Title = "Route Planner",
                    ControllerName = "RoutePlanner",
                    ActionName = "OrderRouting",
                    Visible = true,
                    IconClass = "far fa-dot-circle-o",
                    RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
                };

                var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
                if (pluginNode != null)
                    pluginNode.ChildNodes.Add(menuItem);
                else
                    rootNode.ChildNodes.Add(menuItem);

            return Task.CompletedTask;

        }
    }
}

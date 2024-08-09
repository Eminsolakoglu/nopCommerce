using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Shipping.RoutePlanner.Services;

namespace Nop.Plugin.Shipping.RoutePlanner.Infrastructure
{
    public class NopStartUp : INopStartup
    {
        public int Order => 100;

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
           services.AddScoped<IRoutePlannerService,RoutePlannerService>();
        }
    }
}

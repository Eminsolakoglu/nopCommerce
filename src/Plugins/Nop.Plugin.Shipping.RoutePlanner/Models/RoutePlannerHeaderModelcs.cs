using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Plugins;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Shipping.RoutePlanner.Models
{
    public record RoutePlannerHeaderModelcs:BaseNopModel
    {
        public string WidgetContent { get; set; }
        public int OrderCount { get; set; }
    }
}

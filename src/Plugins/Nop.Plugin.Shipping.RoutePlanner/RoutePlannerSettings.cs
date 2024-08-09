using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.RoutePlanner
{//Widgetzone da neeler buluncak onu burada belirtiyoruz
    public class RoutePlannerSettings: ISettings
    {
        public string WidgetzoneContent { get; set; }
    }
}

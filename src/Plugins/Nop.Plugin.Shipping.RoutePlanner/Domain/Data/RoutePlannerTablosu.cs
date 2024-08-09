using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Shipping.RoutePlanner.Domain.Data
{
    public partial class RoutePlannerTablosu:BaseEntity
    {
        public string State { get; set; }
        public string City { get; set; }
        public int OrderCount { get; set; }
    }
}

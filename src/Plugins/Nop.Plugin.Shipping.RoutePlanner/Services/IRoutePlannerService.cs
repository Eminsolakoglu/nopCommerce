using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.RoutePlanner.Services
{
    public interface IRoutePlannerService
    {
        Task<int> GetOrderCount();
        Task InsertOrderCount();
    }
}

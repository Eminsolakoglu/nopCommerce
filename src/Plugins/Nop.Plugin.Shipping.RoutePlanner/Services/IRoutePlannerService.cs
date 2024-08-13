using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Shipping.RoutePlanner.Models;


namespace Nop.Plugin.Shipping.RoutePlanner.Services
{
    public interface IRoutePlannerService
    {
        public int GetGreenaddressCount();
        public List<Order> GetOrdersNotPickedUp();
        //List<string> GetAdressCounty();
        public List<string> GetCounties();


        public Task<int> GetOrderCount();
        public Task InsertOrderCount(String state);
        Task<List<OrderViewModel>> GetOrdersNotPickedUpWithCounties();
    }
}

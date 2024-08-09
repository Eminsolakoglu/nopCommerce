using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Data;
using Nop.Plugin.Shipping.RoutePlanner.Domain.Data;

namespace Nop.Plugin.Shipping.RoutePlanner.Services
{
    public class RoutePlannerService :IRoutePlannerService
    {
        #region Fields
        private readonly IRepository<RoutePlannerTablosu> _routePlannerTablosuRepository;
        private readonly IRepository<Address> _addressRepository;

        #endregion

        #region Ctor

        public RoutePlannerService(
            IRepository<RoutePlannerTablosu> routePlannerTablosuRepository,
            IRepository<Address> addressRepository
            )
        {
            _routePlannerTablosuRepository = routePlannerTablosuRepository;
            _addressRepository = addressRepository;
        }
        #endregion

        #region Methods

        public async Task<int> GetOrderCount()
        {
            var state = "New York";
            var recordedOrders = _routePlannerTablosuRepository.Table.Where(x=>x.State == state).FirstOrDefault();
            if (recordedOrders != null)
                return recordedOrders.OrderCount;
            else
            {
                await InsertOrderCount();
                recordedOrders = _routePlannerTablosuRepository.Table.Where(x => x.State == state).FirstOrDefault();
                return recordedOrders.OrderCount;

            }
        }
        public async Task InsertOrderCount()
        {
            var state = "New York";
            var orders= _addressRepository.Table.Where(c=>c.City ==state).ToList();
            var currentOrderCount = orders.Count;

            var newrecord = new RoutePlannerTablosu
            {
                State = state,
                OrderCount = currentOrderCount,

            };
            await _routePlannerTablosuRepository.InsertAsync(newrecord);
        }

        #endregion
    }
}

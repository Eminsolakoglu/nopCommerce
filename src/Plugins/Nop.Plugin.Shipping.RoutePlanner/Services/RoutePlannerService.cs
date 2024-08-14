using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Plugin.Shipping.RoutePlanner.Domain.Data;
using Nop.Plugin.Shipping.RoutePlanner.Models;

namespace Nop.Plugin.Shipping.RoutePlanner.Services
{
    public class RoutePlannerService :IRoutePlannerService
    {
        #region Fields
        private readonly IRepository<RoutePlannerTablosu> _routePlannerTablosuRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Order> _orderRepository;

        #endregion

        #region Ctor

        public RoutePlannerService(
            IRepository<RoutePlannerTablosu> routePlannerTablosuRepository,
            IRepository<Address> addressRepository,
            IRepository<Order> orderRepository
            )
        {
            _routePlannerTablosuRepository = routePlannerTablosuRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
        }
        #endregion

        #region Methods
        public int GetGreenaddressCount()
        {
            // PickupInStore 0 olan siparişlerin BillingAddressId'sini al
            var billingAddressIds = _orderRepository.Table
                .Where(o => o.PickupInStore == false)
                .Select(o => o.BillingAddressId)
                .ToList();
            var cityName = "İstanbul";
            var addressCount = _addressRepository.Table
        .Where(a => billingAddressIds.Contains(a.Id) && a.City == cityName)
        .Count();
            return addressCount;
        }
        public async Task<List<OrderViewModel>> GetOrdersNotPickedUpWithCounties()
        {
            var orders = _orderRepository.Table
                .Where(o => !o.PickupInStore)
                .ToList();

            var billingAddressIds = orders
                .Select(o => o.BillingAddressId)
                .Distinct()
                .ToList();

            var addresses = _addressRepository.Table
                .Where(a => billingAddressIds.Contains(a.Id))
                .ToList();

            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id= order.Id,
                CustomOrderNumber = order.CustomOrderNumber,
                PaidDateUtc = order.PaidDateUtc,
                County = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.County ?? "Bilinmiyor",
                Address1 = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.Address1 ?? "Bilinmiyor",
                PhoneNumber = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.PhoneNumber ?? "Bilinmiyor",
                FirstName = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.FirstName ?? "Bilinmiyor",
                LastName = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.LastName ?? "Bilinmiyor",
                ZipPostalCode = addresses.FirstOrDefault(a => a.Id == order.BillingAddressId)?.ZipPostalCode ?? "Bilinmiyor",
                 
            }).ToList();

            return orderViewModels;
        }
        public List<string> GetCounties()
        {
            // PickupInStore=false olan siparişlerin BillingAddressId'sini al
            var billingAddressIds = _orderRepository.Table
                .Where(o => o.PickupInStore == false)
                .Select(o => o.BillingAddressId)
                .ToList();

            // Şehir adı İstanbul olan adreslerin County değerlerini al
            var cityName = "İstanbul";
            var addressCounties = _addressRepository.Table
                .Where(a => billingAddressIds.Contains(a.Id) && a.City == cityName)
                .Select(a => a.County)
                .ToList();

            return addressCounties;
        }

        public List<Order> GetOrdersNotPickedUp()
        {
            return _orderRepository.Table
                .Where(o => !o.PickupInStore)
                .Select(o => new Order
                {
                    CustomOrderNumber = o.CustomOrderNumber,
                    PaidDateUtc = o.PaidDateUtc
                })
                .ToList();
        }

        public async Task<int> GetOrderCount()
        {
            var state = "New York";
            var recordedOrders = _routePlannerTablosuRepository.Table.Where(x=>x.State == state).FirstOrDefault();

            if (recordedOrders != null)
                return recordedOrders.OrderCount;
            else
            {
                await InsertOrderCount(state);
                recordedOrders = _routePlannerTablosuRepository.Table.Where(x => x.State == state).FirstOrDefault();
                return recordedOrders.OrderCount;

            }
        }
        public async Task InsertOrderCount(String state)
        {
            
            var orders= _addressRepository.Table.Where(c=>c.City == state).ToList();
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

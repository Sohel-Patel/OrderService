using MongoDB.Driver;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.BusinessLogicLayer.ServiceContracts;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderResponse?> AddOrder(OrderAddRequest addRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrder(Guid orderID)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderResponse?>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse?> UpdateOrder(OrderUpdateRequest updateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
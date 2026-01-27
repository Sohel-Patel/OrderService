using MongoDB.Driver;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.ServiceContracts
{
    public interface IOrderService
    {
        Task<List<OrderResponse?>> GetOrders();
        Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);

        Task<OrderResponse?> AddOrder(OrderAddRequest addRequest);
        Task<OrderResponse?> UpdateOrder(OrderUpdateRequest updateRequest);
        Task<bool> DeleteOrder(Guid orderID);

    }
}
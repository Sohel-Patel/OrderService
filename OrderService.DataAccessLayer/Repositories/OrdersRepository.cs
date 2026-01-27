using MongoDB.Driver;
using OrderService.DataAccessLayer.Entities;
using OrderService.DataAccessLayer.RepositoryContracts;

namespace OrderService.DataAccessLayer.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly string collectionName = "orders";
        public OrdersRepository(IMongoDatabase mongoDatabase)
        {
            _orderCollection = mongoDatabase.GetCollection<Order>(collectionName);
        }
        public async Task<Order?> AddOrder(Order order)
        {
            order.OrderID = Guid.NewGuid();
            await _orderCollection.InsertOneAsync(order);
            return order;
        }

        public async Task<bool> DeleteOrder(Guid orderID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(x => x.OrderID,orderID);
            DeleteResult deleteResult = await _orderCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0;
        }

        public async Task<Order> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            Order existingOrder = await (await _orderCollection.FindAsync(filter)).FirstOrDefaultAsync();
            return existingOrder;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await(await _orderCollection.FindAsync(Builders<Order>.Filter.Empty)).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            return await(await _orderCollection.FindAsync(filter)).ToListAsync();
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(x => x.OrderID,order.OrderID);
            Order? existingOrder = await(await _orderCollection.FindAsync(filter)).FirstOrDefaultAsync();
            if (existingOrder == null)
            {
                return null;
            }
            ReplaceOneResult replaceOneResult = await _orderCollection.ReplaceOneAsync(filter,order);
            return order;
        }
    }
}
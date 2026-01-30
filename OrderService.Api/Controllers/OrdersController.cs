using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.BusinessLogicLayer.ServiceContracts;
using OrderService.BusinessLogicLayer.Validators;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController: ControllerBase
    {
        private readonly IOrdersService _ordersService;
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        //GET:  /api/orders
        [HttpGet]
        public async Task<IEnumerable<OrderResponse?>> Get()
        {
            List<OrderResponse?> orders = await _ordersService.GetOrders();
            return orders;
        }

        //POST:  /api/orders
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderAddRequest addRequest)
        {
            if (addRequest == null)
            {
                return BadRequest("Invalid order data");
            }
            OrderResponse? order = await _ordersService.AddOrder(addRequest);
            if (order == null)
            {
                return BadRequest();
            }
            return Created($"/api/orders/search/orderid/{order.OrderID}",order);
        }

        //PUT:  /api/orders
        [HttpPut("{orderID}")]
        public async Task<IActionResult> UpdateOrder([FromRoute]Guid orderID,OrderUpdateRequest updateRequest)
        {
            if (updateRequest == null)
            {
                return BadRequest("Invalid order data");
            }
            if (orderID != updateRequest.OrderID)
            {
                return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request Body");
            }
            OrderResponse? order = await _ordersService.UpdateOrder(updateRequest);
            if (order == null)
            {
                return Problem("Error in updating order");
            }
            return Ok(order);
        }

        //DELETE: /api/orders/{orderID}
        [HttpDelete("/{orderID}")]
        public async Task<IActionResult> DeleteOrder([FromRoute]Guid orderID)
        {
            bool result = await _ordersService.DeleteOrder(orderID);
            if (!result)
            {
                return Problem("Error in deleting order.");
            }
            return Ok(result);
        }


        //GET:  /api/orders/search/orderid/{orderID}
        [HttpGet("/search/orderid/{orderID}")]
        public async Task<OrderResponse?> Get(Guid orderID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(x => x.OrderID,orderID);
            OrderResponse? order = await _ordersService.GetOrderByCondition(filter);
            return order;
        }

        //GET:  /api/orders/search/productid/{productID}
        [HttpGet("/search/productid/{productID}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(x => x.OrderItems,Builders<OrderItem>.Filter.Eq(x => x.ProductID,productID));
            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
            return orders;
        }


        
        //GET:  /api/orders/search/orderDate/{orderDate}
        [HttpGet("/search/orderDate/{orderDate}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(x => x.OrderDate.ToString("yyyy-MM-dd"),orderDate.ToString("yyyy-MM-dd"));
            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
            return orders;
        }


    }
}
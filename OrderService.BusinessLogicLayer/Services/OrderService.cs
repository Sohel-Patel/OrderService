using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.BusinessLogicLayer.ServiceContracts;
using OrderService.DataAccessLayer.Entities;
using OrderService.DataAccessLayer.RepositoryContracts;

namespace OrderService.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IMapper _mapper; 
        private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
        private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
        private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
        private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
        public OrderService(IOrdersRepository ordersRepo,
            IMapper mapper, 
            IValidator<OrderAddRequest> orderAddRequestValidator,
            IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
            IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
            IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator)
        {
            _ordersRepo = ordersRepo;
            _mapper = mapper;
            _orderAddRequestValidator = orderAddRequestValidator;
            _orderItemAddRequestValidator = orderItemAddRequestValidator;
            _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
            _orderUpdateRequestValidator = orderUpdateRequestValidator;
        }
        public async Task<OrderResponse?> AddOrder(OrderAddRequest addRequest)
        {
            if (addRequest == null)
            {
                throw new ArgumentNullException(nameof(addRequest));
            }

            ValidationResult orderAddRequestResult = await _orderAddRequestValidator.ValidateAsync(addRequest);
            if (!orderAddRequestResult.IsValid)
            {
                string errors = string.Join(", ",orderAddRequestResult.Errors.Select(x => x.ErrorMessage));
                throw new ArgumentException(errors);
            }

            //validate each order item.
            foreach (OrderItemAddRequest item in addRequest.OrderItems)
            {
                ValidationResult orderItemAddRequestResult = await _orderItemAddRequestValidator.ValidateAsync(item);
                if (!orderItemAddRequestResult.IsValid)
                {
                    string errors = string.Join(", ",orderItemAddRequestResult.Errors.Select(x => x.ErrorMessage));
                    throw new ArgumentException(errors);
                }
            }

            //checking weather userid exist or not..

            //convert OrderAddRequest To Order..
            Order orderInput = _mapper.Map<Order>(addRequest);

            foreach (OrderItem item in orderInput.OrderItems)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;
            }
            orderInput.TotalBill = orderInput.OrderItems.Sum(x => x.TotalPrice);

            Order? addedOrder = await _ordersRepo.AddOrder(orderInput);
            if (addedOrder == null)
            {
                return null;
            }
            return _mapper.Map<OrderResponse>(addedOrder);
        }

        public async Task<bool> DeleteOrder(Guid orderID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(x => x.OrderID,orderID);
            Order? existingOrder = await _ordersRepo.GetOrderByCondition(filter);
            if (existingOrder == null)
            {
                return false;
            }

            bool isDeleted = await _ordersRepo.DeleteOrder(orderID);
            return isDeleted;
        }

        public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            Order? existingOrder = await _ordersRepo.GetOrderByCondition(filter);
            if (existingOrder == null)
            {
                return null;
            }
            return _mapper.Map<OrderResponse>(existingOrder);
        }

        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<Order?> orders = await _ordersRepo.GetOrders();
            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return orderResponses.ToList();
        }

        public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            IEnumerable<Order> orders = await _ordersRepo.GetOrdersByCondition(filter);
            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return orderResponses.ToList();
        }

        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }

            ValidationResult orderUpdateRequestResult = await _orderUpdateRequestValidator.ValidateAsync(updateRequest);
            if (!orderUpdateRequestResult.IsValid)
            {
                string errors = string.Join(", ",orderUpdateRequestResult.Errors.Select(x => x.ErrorMessage));
                throw new ArgumentException(errors);
            }

            //validate each order item.
            foreach (OrderItemUpdateRequest item in updateRequest.OrderItems)
            {
                ValidationResult orderItemUpdateRequestResult = await _orderItemUpdateRequestValidator.ValidateAsync(item);
                if (!orderItemUpdateRequestResult.IsValid)
                {
                    string errors = string.Join(", ",orderItemUpdateRequestResult.Errors.Select(x => x.ErrorMessage));
                    throw new ArgumentException(errors);
                }
            }

            //checking weather userid exist or not..

            //convert OrderAddRequest To Order..
            Order orderInput = _mapper.Map<Order>(updateRequest);

            foreach (OrderItem item in orderInput.OrderItems)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;
            }
            orderInput.TotalBill = orderInput.OrderItems.Sum(x => x.TotalPrice);

            Order? updatedOrder = await _ordersRepo.UpdateOrder(orderInput);
            if (updatedOrder == null)
            {
                return null;
            }
            return _mapper.Map<OrderResponse>(updatedOrder);
        }
    }
}
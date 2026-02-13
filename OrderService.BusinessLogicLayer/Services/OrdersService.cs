using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.BusinessLogicLayer.HttpClients;
using OrderService.BusinessLogicLayer.ServiceContracts;
using OrderService.DataAccessLayer.Entities;
using OrderService.DataAccessLayer.RepositoryContracts;

namespace OrderService.BusinessLogicLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IMapper _mapper; 
        private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
        private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
        private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
        private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
        private readonly UsersMicroserviceClient _usersMicroserviceClient;
        private readonly ProductsMicroserviceClient _productsMicroserviceClient;
        public OrdersService(IOrdersRepository ordersRepo,
            IMapper mapper, 
            IValidator<OrderAddRequest> orderAddRequestValidator,
            IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
            IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
            IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator,
            UsersMicroserviceClient usersMicroserviceClient,
            ProductsMicroserviceClient productsMicroserviceClient)
        {
            _ordersRepo = ordersRepo;
            _mapper = mapper;
            _orderAddRequestValidator = orderAddRequestValidator;
            _orderItemAddRequestValidator = orderItemAddRequestValidator;
            _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
            _orderUpdateRequestValidator = orderUpdateRequestValidator;
            _usersMicroserviceClient = usersMicroserviceClient;
            _productsMicroserviceClient = productsMicroserviceClient;
        }
        public async Task<OrderResponse?> AddOrder(OrderAddRequest addRequest)
        {
            if (addRequest == null)
            {
                throw new ArgumentNullException(nameof(addRequest));
            }
            
            //validation using fluent validation
            ValidationResult orderAddRequestResult = await _orderAddRequestValidator.ValidateAsync(addRequest);
            if (!orderAddRequestResult.IsValid)
            {
                string errors = string.Join(", ",orderAddRequestResult.Errors.Select(x => x.ErrorMessage));
                throw new ArgumentException(errors);
            }

            List<ProductDTO?> products = new List<ProductDTO?>();
            //validate each order item.
            foreach (OrderItemAddRequest item in addRequest.OrderItems)
            {
                ValidationResult orderItemAddRequestResult = await _orderItemAddRequestValidator.ValidateAsync(item);
                if (!orderItemAddRequestResult.IsValid)
                {
                    string errors = string.Join(", ",orderItemAddRequestResult.Errors.Select(x => x.ErrorMessage));
                    throw new ArgumentException(errors);
                }
                //checking product id exist in database
                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(item.ProductID);
                if (product == null)
                {
                    throw new ArgumentException("Invalid product in order");
                }
                products.Add(product);
            }

            //checking weather userid exist or not..
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(addRequest.UserID);
            if (user == null)
                throw new ArgumentException("Invalid UserID");
            
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
            
            OrderResponse orderResponse = _mapper.Map<OrderResponse>(addedOrder);
            foreach (var orderItem in orderResponse.OrderItems)
            {
                ProductDTO? product = products.FirstOrDefault(x => x != null && x.ProductId == orderItem.ProductID);
                if (product == null)
                    continue;
                
                _mapper.Map<ProductDTO,OrderItemResponse>(product,orderItem);
            }
            
            return orderResponse;
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
            
            OrderResponse orderResponse = _mapper.Map<OrderResponse>(existingOrder);
            foreach (var orderItem in orderResponse.OrderItems)
            {
                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItem.ProductID);
                if (product == null)
                    continue;
                
                _mapper.Map<ProductDTO,OrderItemResponse>(product,orderItem);
            }
            return orderResponse;
        }

        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<Order?> orders = await _ordersRepo.GetOrders();
            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            foreach (var item in orderResponses)
            {
                if (item == null)
                    continue;
                
                foreach (var orderItem in item.OrderItems)
                {
                    ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItem.ProductID);
                    if (product == null)
                        continue;
                    
                    _mapper.Map<ProductDTO,OrderItemResponse>(product,orderItem);
                }
            }
            return orderResponses.ToList();
        }

        public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            IEnumerable<Order> orders = await _ordersRepo.GetOrdersByCondition(filter);
            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            foreach (var item in orderResponses)
            {
                if (item == null)
                    continue;
                
                foreach (var orderItem in item.OrderItems)
                {
                    ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItem.ProductID);
                    if (product == null)
                        continue;
                    
                    _mapper.Map<ProductDTO,OrderItemResponse>(product,orderItem);
                }
            }
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

            List<ProductDTO?> products = new List<ProductDTO?>();
            //validate each order item.
            foreach (OrderItemUpdateRequest item in updateRequest.OrderItems)
            {
                ValidationResult orderItemUpdateRequestResult = await _orderItemUpdateRequestValidator.ValidateAsync(item);
                if (!orderItemUpdateRequestResult.IsValid)
                {
                    string errors = string.Join(", ",orderItemUpdateRequestResult.Errors.Select(x => x.ErrorMessage));
                    throw new ArgumentException(errors);
                }
                //checking product id exist in database
                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(item.ProductID);
                if (product == null)
                {
                    throw new ArgumentException("Invalid product in order");
                }
                products.Add(product);
            }

            //checking weather userid exist or not..
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(updateRequest.UserID);
            if (user == null)
                throw new ArgumentException("Invalid UserID");
            
            
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
            OrderResponse orderResponse = _mapper.Map<OrderResponse>(updatedOrder);
            foreach (var orderItem in orderResponse.OrderItems)
            {
                ProductDTO? product = products.FirstOrDefault(x => x != null && x.ProductId == orderItem.ProductID);
                if (product == null)
                    continue;
                
                _mapper.Map<ProductDTO,OrderItemResponse>(product,orderItem);
            }
            return orderResponse;
        }
    }
}
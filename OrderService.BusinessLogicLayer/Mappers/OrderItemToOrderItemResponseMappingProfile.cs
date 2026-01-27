using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class OrderItemToOrderItemResponseMappingProfile: Profile
    {
        public OrderItemToOrderItemResponseMappingProfile()
        {
            CreateMap<OrderItem,OrderItemResponse>()
            .ForMember(x => x.ProductID,options => options.MapFrom(x => x.ProductID))
            .ForMember(x => x.Quantity,options => options.MapFrom(x => x.Quantity))
            .ForMember(x => x.TotalPrice,options => options.MapFrom(x => x.TotalPrice))
            .ForMember(x => x.UnitPrice,options => options.MapFrom(x => x.UnitPrice));
        }
    }
}
using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class OrderItemUpdateRequestToOrderItemMappingProfile : Profile
    {
        public OrderItemUpdateRequestToOrderItemMappingProfile()
        {
            CreateMap<OrderItemAddRequest,OrderItem>()
            .ForMember(x => x.ProductID,options => options.MapFrom(x => x.ProductID))
            .ForMember(x => x.UnitPrice,options => options.MapFrom(x => x.UnitPrice))
            .ForMember(x => x.Quantity,options => options.MapFrom(x => x.Quantity))
            .ForMember(x => x._id,options => options.Ignore())
            .ForMember(x => x.TotalPrice,options => options.Ignore());
        }
    }
}
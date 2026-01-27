using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class OrderUpdateRequestToOrderMappingProfile : Profile
    {
        public OrderUpdateRequestToOrderMappingProfile()
        {
            CreateMap<OrderUpdateRequest,Order>()
            .ForMember(x => x.UserID,options => options.MapFrom(x => x.UserID))
            .ForMember(x => x.OrderDate,options => options.MapFrom(x => x.OrderDate))
            .ForMember(x => x.OrderItems,options => options.MapFrom(x => x.OrderItems))
            .ForMember(x => x.OrderID,options => options.MapFrom(x => x.OrderID))
            .ForMember(x => x.TotalBill,options => options.Ignore())
            .ForMember(x => x._id,options => options.Ignore());

        }
    }
}
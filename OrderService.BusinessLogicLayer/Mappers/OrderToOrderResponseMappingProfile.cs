using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;
using OrderService.DataAccessLayer.Entities;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class OrderToOrderResponseMappingProfile: Profile
    {
        public OrderToOrderResponseMappingProfile()
        {
            CreateMap<Order,OrderResponse>()
            .ForMember(x => x.OrderID,options => options.MapFrom(x => x.OrderID))
            .ForMember(x => x.OrderDate,options => options.MapFrom(x => x.OrderDate))
            .ForMember(x => x.UserID,options => options.MapFrom(x => x.UserID))
            .ForMember(x => x.TotalBill,options => options.MapFrom(x => x.TotalBill))
            .ForMember(x => x.OrderItems,options => options.MapFrom(x => x.OrderItems));
        }
    }
}
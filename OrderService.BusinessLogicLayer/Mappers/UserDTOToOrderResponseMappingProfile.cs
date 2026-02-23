using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class UserDTOToOrderResponseMappingProfile : Profile
    {
        public UserDTOToOrderResponseMappingProfile()
        {
            CreateMap<UserDTO,OrderResponse>()
            .ForMember(x => x.UserPersonName,options => options.MapFrom(x => x.PersonName))
            .ForMember(x => x.Email,options => options.MapFrom(x => x.Email));
        }
    }
}
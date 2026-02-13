using AutoMapper;
using OrderService.BusinessLogicLayer.DTO;

namespace OrderService.BusinessLogicLayer.Mappers
{
    public class ProducvtDTOToOrderItemResponseMappingProdile : Profile
    {
        public ProducvtDTOToOrderItemResponseMappingProdile()
        {
            CreateMap<ProductDTO,OrderItemResponse>()
            .ForMember(x => x.ProductName,options => options.MapFrom(x => x.ProductName))
            .ForMember(x => x.Category,options => options.MapFrom(x => x.Category));
        }
    }
}
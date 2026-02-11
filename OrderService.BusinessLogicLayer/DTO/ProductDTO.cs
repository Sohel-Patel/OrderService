namespace OrderService.BusinessLogicLayer.DTO
{
    public record ProductDTO(Guid ProductId,string ProductName,string Category,double? UnitPrice,int? QuantityInStock);
}
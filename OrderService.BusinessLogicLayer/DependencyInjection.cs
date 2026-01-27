using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.BusinessLogicLayer.Mappers;
using OrderService.BusinessLogicLayer.ServiceContracts;
using OrderService.BusinessLogicLayer.Services;
using OrderService.BusinessLogicLayer.Validators;

namespace OrderService.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            services.AddScoped<IOrdersService,OrdersService>();
            return services;
        }
    }
}
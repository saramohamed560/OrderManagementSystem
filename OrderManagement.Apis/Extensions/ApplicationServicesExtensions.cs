using Microsoft.AspNetCore.Mvc;
using OrderManagement.Apis.Errors;
using OrderManagement.Apis.Helpers;
using OrderManagement.Core;
using OrderManagement.Core.Services.Contracts;
using OrderManagement.Repository;
using OrderManagement.Services;

namespace OrderManagement.Apis.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Configure validation error response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Count() > 0)
                                                        .SelectMany(m => m.Value.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToList();
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IInvoiceService), typeof(InvoiceService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderServicecs));
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}

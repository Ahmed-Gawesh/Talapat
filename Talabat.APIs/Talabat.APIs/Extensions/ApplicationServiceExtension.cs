using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Repository.Repositories;
using Talabat.Repository;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Services;
using Talabat.Service;
using Stripe;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IPaymentService, PaymentService>();

            Services.AddAutoMapper(typeof(MappingProfiles));


            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var erros = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                            .SelectMany(E => E.Value.Errors)
                                            .Select(E => E.ErrorMessage).ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = erros
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return Services;

        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using SmartPayPark.Api.Services;
using SmartPayPark.Application.Authentication.Interfaces.Services;
using SmartPayPark.Application.Authentication.Services;
using SmartPayPark.Application.Operators.Interfaces.Services;

namespace SmartPayPark.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOperatorService, OperatorService>();

        return services;
    }
}
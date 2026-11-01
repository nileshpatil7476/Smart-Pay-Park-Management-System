using Microsoft.Extensions.DependencyInjection;
using SmartPayPark.Api.Services;
using SmartPayPark.Application.Admin.Documents.Interfaces.Services;
using SmartPayPark.Application.Admin.Documents.Services;
using SmartPayPark.Application.Admin.Operators.Interfaces;
using SmartPayPark.Application.Admin.Operators.Services;
using SmartPayPark.Application.Authentication.Interfaces.Services;
using SmartPayPark.Application.Authentication.Services;
using SmartPayPark.Application.Operators.Interfaces.Services;
using SmartPayPark.Application.Operators.Services;

namespace SmartPayPark.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<IAdminOperatorService, AdminOperatorService>();
        services.AddScoped<IAdminDocumentService, AdminDocumentService>();
        services.AddScoped<IOperatorDocumentService, OperatorDocumentService>();

        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartPayPark.Api.Services;
using SmartPayPark.Application.Admin.Documents.Interfaces.Repositories;
using SmartPayPark.Application.Admin.Operators.Interfaces;
using SmartPayPark.Application.Authentication.Interfaces.Repositories;
using SmartPayPark.Application.Authentication.Interfaces.Services;
using SmartPayPark.Application.Common.Interfaces;
using SmartPayPark.Application.Operators.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Interfaces.Repositories;
using SmartPayPark.Infrastructure.FileStorage;
using SmartPayPark.Infrastructure.Persistence;
using SmartPayPark.Infrastructure.Persistence.Repositories;

namespace SmartPayPark.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database connection
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.GetName().Name)));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOperatorRepository, OperatorRepository>();
        services.AddScoped<IAdminOperatorRepository, AdminOperatorRepository>();
        services.AddScoped<IAdminDocumentRepository, AdminDocumentRepository>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IOperatorDocumentRepository, OperatorDocumentRepository>();

        // JWT service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
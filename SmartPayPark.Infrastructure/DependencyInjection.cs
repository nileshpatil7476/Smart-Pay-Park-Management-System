using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartPayPark.Domain.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Api.Services;
using SmartPayPark.Application.Authentication.Interfaces.Services;
using SmartPayPark.Infrastructure.Persistence.Repositories;
using SmartPayPark.Infrastructure.Persistence;
using SmartPayPark.Application.Authentication.Interfaces.Repositories;

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
        //services.AddScoped<IAdminOperatorRepository, AdminOperatorRepository>();

        // JWT service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
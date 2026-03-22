using Application.Auth;
using Application.Auth.Persistence.Repositories;
using Application.Auth.Token;
using Application.Common.Interfaces;
using backend.Services;
using Infrastructure.Auth.Options;
using Infrastructure.Auth.Persistence.Repositories;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDatabase(configuration);
        services.AddAppIdentity();
        services.AddAppOptions();
        services.AddRepositories();
        services.AddAppUnitOfWork();
        services.AddAppJwt();
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddAppOptions(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateOnStart();
    }

    private static void AddAppJwt(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
    }

    private static void AddAppUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, AppDbContext>();
    }

    private static void AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Unexisting connection string");
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }

    private static void AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<AuthUser>().AddEntityFrameworkStores<AppDbContext>();
    }

}
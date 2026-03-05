using System.Net.Security;
using Infrastructure.Persistance;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Unexisting connection string");
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }
}
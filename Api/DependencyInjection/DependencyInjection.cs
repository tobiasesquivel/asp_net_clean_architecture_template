using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using api.BackgroundServices;
using api.BackgroundServices.Jobs;
using api.Interceptors;
using api.Interfaces;
using api.Middlewares;
using api.Options;
using api.Persistence;
using api.Persistence.Identity;
using api.Persistence.Repositories;
using api.Persistence.Repositories.Interfaces;
using api.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Wolverine;
using Wolverine.FluentValidation;

namespace api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IHostBuilder host, IConfiguration config)
    {
        services.AddAppLogger(host);
        services.AddAppExceptionMiddlewares();
        services.AddAppPersistence(config);
        services.AddAppWolverine(host);
        services.AddAppValidators();
        services.AddAppRepositories();
        services.AddAppUnitOfWork();
        services.AddScoped<IdentityService>();
        services.AddAppChannels();
        services.AddAppBackgroundServices();
        services.AddAppOptions(config);
        services.AddAppServices();
        return services;
    }

    private static IServiceCollection AddAppLogger(this IServiceCollection services, IHostBuilder host)
    {
        host.UseSerilog();
        return services;
    }

    private static IServiceCollection AddAppPersistence(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Unexisting string connection");
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }).AddIdentityCore<AppIdentityUser>().AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }

    private static IServiceCollection AddAppWolverine(this IServiceCollection services, IHostBuilder host)
    {
        host.UseWolverine(opts =>
        {
            opts.UseFluentValidation();
        }, ExtensionDiscovery.ManualOnly);
        return services;
    }


    private static IServiceCollection AddAppValidators(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
        return services;
    }

    private static IServiceCollection AddAppRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        return services;
    }

    private static IServiceCollection AddAppUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
    private static IServiceCollection AddAppBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<MediaDeletedGarbageCollector>();
        return services;
    }

    private static IServiceCollection AddAppChannels(this IServiceCollection services)
    {
        BoundedChannelOptions mediaGarbageCollectorOptions = new(1000) { FullMode = BoundedChannelFullMode.Wait };
        var mediaGarbageCollectorChannel = Channel.CreateBounded<MediaDeletedGarbageCollectorJob>(mediaGarbageCollectorOptions);
        services.AddSingleton(mediaGarbageCollectorChannel);
        return services;
    }
    private static IServiceCollection AddAppExceptionMiddlewares(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }
    private static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CloudinaryOptions>(
            config.GetSection("Cloudinary")
        );
        return services;
    }

    private static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, CloudinaryStorageService>();
        return services;
    }
}
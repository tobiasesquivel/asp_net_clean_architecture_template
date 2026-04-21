using Serilog;
using api.DependencyInjection;
using FluentValidation;
using System.Text.Json.Serialization;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("JasperFx.CodeGeneration", Serilog.Events.LogEventLevel.Warning)

    .Filter.ByExcluding(logEvent => logEvent.Exception is ValidationException)

    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null, false));
                });

builder.Services.AddDependencyInjection(builder.Host, builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();

using CrossCutting.Enums;
using CrossCutting.Interfaces;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Serilog.Extensions.Logging;
using System;
using System.IO;

namespace WebApi.Modules;

public static class DbModule
{
    public static IServiceCollection AddDbModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string connection)
    {
        IFeatureManager featureManager =
            services.BuildServiceProvider()
                    .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeatureEnum.PostgreSQL))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            var serilogLoggerFactory = new SerilogLoggerFactory();

            var connectionString = string.IsNullOrEmpty(connection) ?
                configuration.GetConnectionString("DefaultConnection") : connection;

            services.AddDbContext<RouteDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.UseLoggerFactory(serilogLoggerFactory);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRouteRepository, RouteRepository>();
        }
        else
        {
            services.AddScoped<IUnitOfWork, CsvUnitOfWork>();
            services.AddScoped<IRouteRepository>(provider => new CsvRouteRepository(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rotas.csv")));
        }

        return services;
    }
}
using Application.UseCases.V1.Route.GetRouteById;
using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using Application.UseCases.V1.Route.PostRoute;
using Application.UseCases.V1.Route.PostRoute.Interfaces;
using CrossCutting.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Modules;

public static class UseCasesModule
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IGetBestRouteByOriginAndDestinyUseCase, GetBestRouteByOriginAndDestinyUseCase>();
        services.AddScoped<IPostRouteUseCase, PostRouteUseCase>();
        services.Decorate<IPostRouteUseCase, PostRouteValidationUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }
}
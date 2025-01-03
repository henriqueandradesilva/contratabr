using Application.UseCases.V1.Route.DeleteRoute;
using Application.UseCases.V1.Route.DeleteRoute.Interfaces;
using Application.UseCases.V1.Route.GetListAllRoute;
using Application.UseCases.V1.Route.GetListAllRoute.Interfaces;
using Application.UseCases.V1.Route.GetListSearchRoute;
using Application.UseCases.V1.Route.GetListSearchRoute.Interfaces;
using Application.UseCases.V1.Route.GetRouteById;
using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using Application.UseCases.V1.Route.PostRoute;
using Application.UseCases.V1.Route.PostRoute.Interfaces;
using Application.UseCases.V1.Route.PutRoute;
using Application.UseCases.V1.Route.PutRoute.Interfaces;
using CrossCutting.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Modules;

public static class UseCasesModule
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IDeleteRouteUseCase, DeleteRouteUseCase>();
        services.AddScoped<IGetListAllRouteUseCase, GetListAllRouteUseCase>();
        services.AddScoped<IGetListSearchRouteUseCase, GetListSearchRouteUseCase>();
        services.AddScoped<IGetRouteByIdUseCase, GetRouteByIdUseCase>();
        services.AddScoped<IGetBestRouteByOriginAndDestinyUseCase, GetBestRouteByOriginAndDestinyUseCase>();
        services.AddScoped<IPostRouteUseCase, PostRouteUseCase>();
        services.Decorate<IPostRouteUseCase, PostRouteValidationUseCase>();
        services.AddScoped<IPutRouteUseCase, PutRouteUseCase>();
        services.Decorate<IPutRouteUseCase, PutRouteValidationUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }
}
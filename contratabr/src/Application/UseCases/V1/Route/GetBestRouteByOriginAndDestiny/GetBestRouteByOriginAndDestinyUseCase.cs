using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Route.GetRouteById;

public class GetBestRouteByOriginAndDestinyUseCase : IGetBestRouteByOriginAndDestinyUseCase
{
    private IOutputPortWithNotFound<string> _outputPort;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetBestRouteByOriginAndDestinyUseCase(
        IRouteRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        string origin,
        string destiny)
    {
        var result =
            await GetBestRoute(origin, destiny);

        if (result == MessageConst.RouteNotExist)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RouteNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public async Task<string> GetBestRoute(string origin, string destiny)
    {
        var routes =
            await _repository.GetAll();

        var bestRoute = FindBestRoute(routes, origin, destiny);

        return bestRoute?.Caminho == null
            ? MessageConst.RouteNotExist
            : string.Join(" - ", bestRoute?.Caminho) + $" ao custo de ${bestRoute?.Valor}";
    }

    private (List<string> Caminho, double Valor)? FindBestRoute(
        List<Domain.Entities.Route> routes,
        string origin,
        string destiny)
    {
        var visited = new HashSet<string>();
        var bestPath = new List<string>();
        var lowestCost = double.MaxValue;

        void Search(string current, List<string> currentPath, double currentCost)
        {
            if (current == destiny)
            {
                if (currentCost < lowestCost)
                {
                    lowestCost = currentCost;
                    bestPath = new List<string>(currentPath);
                }
                return;
            }

            foreach (var route in routes.Where(r => r.Origem == current && !visited.Contains(r.Destino)))
            {
                visited.Add(route.Destino);
                currentPath.Add(route.Destino);
                Search(route.Destino, currentPath, currentCost + route.Valor);
                currentPath.RemoveAt(currentPath.Count - 1);
                visited.Remove(route.Destino);
            }
        }

        visited.Add(origin);
        Search(origin, new List<string> { origin }, 0);

        return bestPath.Count > 0 ? (bestPath, lowestCost) : null;
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<string> outputPort)
        => _outputPort = outputPort;
}

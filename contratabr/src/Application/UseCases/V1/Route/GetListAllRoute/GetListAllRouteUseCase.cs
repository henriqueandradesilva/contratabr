using Application.UseCases.V1.Route.GetListAllRoute.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Route.GetListAllRoute;

public class GetListAllRouteUseCase : IGetListAllRouteUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Route>> _outputPort;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllRouteUseCase(
        IRouteRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var result =
           await _repository.GetAll();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RouteNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Route>> outputPort)
        => _outputPort = outputPort;
}
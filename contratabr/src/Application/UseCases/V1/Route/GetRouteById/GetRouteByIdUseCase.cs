using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Route.GetRouteById;

public class GetRouteByIdUseCase : IGetRouteByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Route> _outputPort;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetRouteByIdUseCase(
        IRouteRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RouteNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Route> outputPort)
        => _outputPort = outputPort;
}
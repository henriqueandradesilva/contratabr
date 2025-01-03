using Application.UseCases.V1.Route.PostRoute.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Route.PostRoute;

public class PostRouteUseCase : IPostRouteUseCase
{
    private IOutputPort<Domain.Entities.Route> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostRouteUseCase(
        IUnitOfWork unitOfWork,
        IRouteRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Route route)
    {
        var normalizedOrigin = route.Origem?.NormalizeString();

        var normalizedDestiny = route.Destino?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Origem.ToUpper().Trim().Contains(normalizedOrigin) &&
                                         c.Destino.ToUpper().Trim().Contains(normalizedDestiny))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.RouteExist);

            _outputPort.Error();

            return;
        }

        if (route.Id == 0)
        {
            route.SetDateCreated();

            await _repository.Add(route)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();

                return;
            }

            _outputPort.Ok(route);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Route> outputPort)
        => _outputPort = outputPort;
}

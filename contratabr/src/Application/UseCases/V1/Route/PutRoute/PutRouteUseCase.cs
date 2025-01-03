using Application.UseCases.V1.Route.PutRoute.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Route.PutRoute;

public class PutRouteUseCase : IPutRouteUseCase
{
    private IOutputPort<Domain.Entities.Route> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutRouteUseCase(
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

        var normalizedDestiny = route.Origem?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != route.Id &&
                                         c.Origem.ToUpper().Trim().Contains(normalizedOrigin) &&
                                         c.Destino.ToUpper().Trim().Contains(normalizedDestiny))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.RouteExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == route.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.RouteNotExist);

                _outputPort.Error();
            }
            else
            {
                route.Map(result);

                result.SetDateUpdated();

                _repository.Update(result);

                var response =
                    await _unitOfWork.Save()
                                     .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response))
                {
                    _notificationHelper.Add(SystemConst.Error, response);

                    _outputPort.Error();
                }
                else
                    _outputPort.Ok(result);
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Route> outputPort)
        => _outputPort = outputPort;
}
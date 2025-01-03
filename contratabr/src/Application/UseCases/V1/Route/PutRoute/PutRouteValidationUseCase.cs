using Application.UseCases.V1.Route.PutRoute.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;

namespace Application.UseCases.V1.Route.PutRoute;

public class PutRouteValidationUseCase : IPutRouteUseCase
{
    private IOutputPort<Domain.Entities.Route> _outputPort;
    private readonly IPutRouteUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutRouteValidationUseCase(
        IPutRouteUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Route route)
    {
        if (route.Invalid())
        {
            var listNotification = route.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(route);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Route> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}
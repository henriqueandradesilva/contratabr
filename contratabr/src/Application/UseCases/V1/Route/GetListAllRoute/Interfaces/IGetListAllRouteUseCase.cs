using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Route.GetListAllRoute.Interfaces;

public interface IGetListAllRouteUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Route>> outputPort);
}
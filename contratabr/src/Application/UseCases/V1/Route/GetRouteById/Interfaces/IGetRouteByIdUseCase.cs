using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Route.GetRouteById.Interfaces;

public interface IGetRouteByIdUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Route> outputPort);
}
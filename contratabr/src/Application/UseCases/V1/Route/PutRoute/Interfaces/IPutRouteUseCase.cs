using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Route.PutRoute.Interfaces;

public interface IPutRouteUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Route route);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Route> outputPort);
}
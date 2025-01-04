using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Route.PostRoute.Interfaces;

public interface IPostRouteUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Route route);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Route> outputPort);
}
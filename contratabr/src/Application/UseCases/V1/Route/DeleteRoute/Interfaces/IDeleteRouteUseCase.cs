using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Route.DeleteRoute.Interfaces;

public interface IDeleteRouteUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Route> outputPort);
}
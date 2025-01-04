using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Route.GetRouteById.Interfaces;

public interface IGetBestRouteByOriginAndDestinyUseCase
{
    System.Threading.Tasks.Task Execute(
        string origin,
        string destiny);

    void SetOutputPort(
        IOutputPortWithNotFound<string> outputPort);
}
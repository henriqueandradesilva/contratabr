using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Route.Response;

public class DeleteRouteResponse : BaseDeleteResponse
{
    public DeleteRouteResponse()
    {

    }

    public DeleteRouteResponse(
        Domain.Entities.Route route)
    {
        Id = route.Id;
    }
}
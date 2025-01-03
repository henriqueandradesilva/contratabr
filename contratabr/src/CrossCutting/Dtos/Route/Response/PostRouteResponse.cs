using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Route.Response;

public class PostRouteResponse : BaseResponse
{
    public PostRouteResponse()
    {

    }

    public PostRouteResponse(
        Domain.Entities.Route route)
    {
        Id = route.Id;
        DataCriacao = route.DateCreated;
        DataAlteracao = route.DateUpdated;
    }
}
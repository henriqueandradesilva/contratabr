using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Route.Response;

public class PutRouteResponse : BaseResponse
{
    public PutRouteResponse()
    {

    }

    public PutRouteResponse(
        Domain.Entities.Route route)
    {
        Id = route.Id;
        DataCriacao = route.DateCreated;
        DataAlteracao = route.DateUpdated;
    }
}
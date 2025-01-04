using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Route.Response;

public class GetRouteResponse : BaseResponse
{
    public string Origem { get; set; }

    public string Destino { get; set; }

    public double Valor { get; set; }

    public GetRouteResponse()
    {

    }

    public GetRouteResponse GetRoute(
        Domain.Entities.Route route)
    {
        if (route == null)
            return null;

        GetRouteResponse getRouteResponse = new GetRouteResponse();
        getRouteResponse.Id = route.Id;
        getRouteResponse.Origem = route.Origem;
        getRouteResponse.Destino = route.Destino;
        getRouteResponse.Valor = route.Valor;
        getRouteResponse.DataCriacao = route.DateCreated;
        getRouteResponse.DataAlteracao = route.DateUpdated;

        return getRouteResponse;
    }
}
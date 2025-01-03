using CrossCutting.Common.Dtos.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Route.Response;

public class GetListRouteResponse : BaseResponse
{
    public string Origem { get; set; }

    public string Destino { get; set; }

    public double Valor { get; set; }

    public GetListRouteResponse()
    {

    }

    public List<GetListRouteResponse> GetListRoute(
        List<Domain.Entities.Route> listRoute)
    {
        if (listRoute == null)
            return null;

        return listRoute
        .Select(e => new GetListRouteResponse()
        {
            Id = e.Id,
            Origem = e.Origem,
            Destino = e.Destino,
            Valor = e.Valor,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}
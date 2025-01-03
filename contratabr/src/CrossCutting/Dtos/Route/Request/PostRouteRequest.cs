using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Route.Request;

public class PostRouteRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.OriginRequired)]
    public string Origem { get; set; }

    [Required(ErrorMessage = MessageConst.DestinyRequired)]
    public string Destino { get; set; }

    [Required(ErrorMessage = MessageConst.ValueRequired)]
    public double Valor { get; set; }

    public PostRouteRequest()
    {

    }

    public PostRouteRequest(
         string origem,
         string destino,
         double valor)
    {
        Origem = origem;
        Destino = destino;
        Valor = valor;
    }
}
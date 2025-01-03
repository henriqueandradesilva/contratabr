using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Route.GetRouteById;

public class GetBestRouteByOriginAndDestinyUseCase : IGetBestRouteByOriginAndDestinyUseCase
{
    private const string _caminhoArquivo = "rotas.csv";
    private IOutputPortWithNotFound<string> _outputPort;
    private readonly IRouteRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetBestRouteByOriginAndDestinyUseCase(
        IRouteRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        string origin,
        string destiny)
    {
        var result =
            await ConsultarMelhorRota(origin, destiny);

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RouteNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public async Task<string> ConsultarMelhorRota(string origem, string destino)
    {
        var rotas =
            await _repository.GetAll();

        var melhorRota = EncontrarMelhorRota(rotas, origem, destino);

        return melhorRota?.Caminho == null
            ? "Rota não encontrada."
            : string.Join(" - ", melhorRota?.Caminho) + $" ao custo de ${melhorRota?.Custo}";
    }

    private (List<string> Caminho, double Custo)? EncontrarMelhorRota(
        List<Domain.Entities.Route> rotas,
        string origem,
        string destino)
    {
        var visitados = new HashSet<string>();
        var melhorCaminho = new List<string>();
        var menorCusto = double.MaxValue;

        void Buscar(string atual, List<string> caminhoAtual, double custoAtual)
        {
            if (atual == destino)
            {
                if (custoAtual < menorCusto)
                {
                    menorCusto = custoAtual;
                    melhorCaminho = new List<string>(caminhoAtual);
                }
                return;
            }

            foreach (var rota in rotas.Where(r => r.Origem == atual && !visitados.Contains(r.Destino)))
            {
                visitados.Add(rota.Destino);
                caminhoAtual.Add(rota.Destino);
                Buscar(rota.Destino, caminhoAtual, custoAtual + rota.Valor);
                caminhoAtual.RemoveAt(caminhoAtual.Count - 1);
                visitados.Remove(rota.Destino);
            }
        }

        visitados.Add(origem);
        Buscar(origem, new List<string> { origem }, 0);

        return melhorCaminho.Count > 0 ? (melhorCaminho, menorCusto) : null;
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<string> outputPort)
        => _outputPort = outputPort;
}
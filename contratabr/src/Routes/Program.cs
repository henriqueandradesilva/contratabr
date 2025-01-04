using Application.UseCases.V1.Route.GetRouteById;
using CrossCutting.Helpers;
using Infrastructure.DataAccess;
using Routes.Common;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Por favor, informe o arquivo CSV.");

            return;
        }

        var csvFilePath = args[0];
        var repository = new CsvRouteRepository(csvFilePath);
        var notificationHelper = new NotificationHelper();
        var getBestRouteByOriginAndDestinyUseCase = new GetBestRouteByOriginAndDestinyUseCase(repository, notificationHelper);

        var outputPort = new ConsoleOutputPort();
        getBestRouteByOriginAndDestinyUseCase.SetOutputPort(outputPort);

        while (true)
        {
            Console.Write("Digite a rota exemplo: GRU-CDG:");

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                break;

            var parts = input.Split('-');

            if (parts.Length != 2)
            {
                Console.WriteLine("Formato inválido. Tente novamente.");

                continue;
            }

            await getBestRouteByOriginAndDestinyUseCase.Execute(parts[0], parts[1]);
        }
    }
}
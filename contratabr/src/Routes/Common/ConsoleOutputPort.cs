using CrossCutting.Interfaces;

namespace Routes.Common;

public class ConsoleOutputPort : IOutputPortWithNotFound<string>
{
    public void Ok(
        string result)
    {
        Console.WriteLine($"Melhor Rota: {result}");
    }

    public void NotFound()
    {
        Console.WriteLine("Rota não encontrada.");
    }

    public void Error()
    {
        throw new NotImplementedException();
    }
}
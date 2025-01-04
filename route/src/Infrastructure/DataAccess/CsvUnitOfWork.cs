using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess;

public sealed class CsvUnitOfWork : IUnitOfWork
{
    public async Task<string> Save()
    {
        return string.Empty;
    }
}
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class RouteRepository : Repository<Route>, IRouteRepository
{
    public RouteRepository(
        ContrataBRDbContext context) : base(context)
    {
    }
}
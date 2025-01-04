using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class RouteRepository : Repository<Route>, IRouteRepository
{
    public RouteRepository(
        RouteDbContext context) : base(context)
    {
    }
}
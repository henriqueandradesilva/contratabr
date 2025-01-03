using Domain.Entities;
using Infrastructure.DataAccess;

namespace tests.Common;

public static class SeedMockData
{
    public static async Task Init(
        ContrataBRDbContext dbContext,
        bool initRoute)
    {
        if (initRoute)
        {
            var routes = new List<Route>
            {
               new Route(1, "GRU", "BRC", 10.00),
               new Route(2, "BRC", "SCL", 5.00),
               new Route(3, "GRU", "CDG", 75.00),
               new Route(4, "GRU", "SCL", 20.00),
               new Route(5, "GRU", "ORL", 56.00),
               new Route(6, "ORL", "CDG", 5.00),
               new Route(7, "SCL", "ORL", 20.00)
            };

            dbContext.Set<Route>().AddRange(routes);
        }

        await dbContext.SaveChangesAsync();
    }
}
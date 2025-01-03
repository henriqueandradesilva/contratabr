using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.DataAccess.Seeds;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        // Routes
        builder.Entity<Route>()
               .HasData(
                    new Route(1, "GRU", "BRC", 10.00),
                    new Route(2, "BRC", "SCL", 5.00),
                    new Route(3, "GRU", "CDG", 75.00),
                    new Route(4, "GRU", "SCL", 20.00),
                    new Route(5, "GRU", "ORL", 56.00),
                    new Route(6, "ORL", "CDG", 5.00),
                    new Route(7, "SCL", "ORL", 20.00)
               );
    }
}
using Infrastructure.DataAccess.Seeds;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.DataAccess;

public sealed class ContrataBRDbContext : DbContext
{
    public ContrataBRDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContrataBRDbContext).Assembly);

        SeedData.Seed(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}
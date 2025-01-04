using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess;

public class CsvRouteRepository : IRouteRepository
{
    private readonly string _csvFilePath;
    private List<Route> _routes;

    public CsvRouteRepository(
        string csvFilePath)
    {
        _csvFilePath = csvFilePath;
        _routes = new List<Route>();
        LoadRoutesFromCsv();
    }

    public async Task Add(
        Route entity)
    {
        _routes.Add(entity);

        await SaveAsync();
    }

    public async Task Add(
        IEnumerable<Route> items)
    {
        _routes.AddRange(items);

        await SaveAsync();
    }

    public void Update(
        Route entity)
    {
        throw new NotSupportedException();
    }

    public void Update(
        IEnumerable<Route> items)
    {
        foreach (var item in items)
            Update(item);
    }

    public void Delete(Route entity)
    {
        _routes.RemoveAll(r => r.Origem == entity.Origem &&
                               r.Destino == entity.Destino);

        Save();
    }

    public async Task ExecuteSqlRawAsync(
        string sql)
    {
        throw new NotSupportedException();
    }

    public Task<bool> Any(
        Expression<Func<Route, bool>> predicate)
    {
        var exists = _routes.AsQueryable().Any(predicate);

        return Task.FromResult(exists);
    }

    public IQueryable<Route> Where(
        Expression<Func<Route, bool>> expression)
    {
        return _routes.AsQueryable().Where(expression);
    }

    public async Task<List<Route>> GetAll()
    {
        return await Task.FromResult(_routes.ToList());
    }

    public IQueryable<Route> GetAllWithIncludes(
        params Expression<Func<Route, object>>[] includes)
    {
        return _routes.AsQueryable();
    }

    public IQueryable<Route> FromSqlRaw(
        string sql)
    {
        throw new NotSupportedException();
    }

    public List<Route> RawSqlQuery(
        string query, Func<DbDataReader, Route> map)
    {
        throw new NotSupportedException();
    }

    public long Count()
    {
        return _routes.LongCount();
    }

    private void LoadRoutesFromCsv()
    {
        if (!File.Exists(_csvFilePath))
        {
            _routes = new List<Route>();

            return;
        }

        _routes = File.ReadAllLines(_csvFilePath)
                      .Skip(1)
                      .Where(line => !string.IsNullOrWhiteSpace(line))
                      .Select(line =>
                      {
                          var parts = line.Split(',');
                          return new Route(0, parts[0], parts[1], double.Parse(parts[2]));
                      })
                      .ToList();
    }

    private async Task SaveAsync()
    {
        var lines = _routes.Select(r => $"{r.Origem},{r.Destino},{r.Valor}");

        await File.WriteAllLinesAsync(_csvFilePath, new[] { "Origem,Destino,Valor" }.Concat(lines));
    }

    private void Save()
    {
        var lines = _routes.Select(r => $"{r.Origem},{r.Destino},{r.Valor}");

        File.WriteAllLines(_csvFilePath, new[] { "Origem,Destino,Valor" }.Concat(lines));
    }
}
using Application.UseCases.V1.Route.GetListSearchRoute;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.RouteUseCasesTests;

public class GetListSearchRouteUseCaseTests
{
    /// <summary>
    /// Listagem de rotas
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Filtered_Routes_By_Origin_And_Destiny_And_Pagination()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ContrataBRDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new ContrataBRDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var mockRouteRepository = new Mock<IRouteRepository>();
        mockRouteRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Route, bool>>>()))
            .Returns((Expression<Func<Route, bool>> predicate) =>
                dbContext.Set<Route>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Route>>>();

        var useCase = new GetListSearchRouteUseCase(
            mockRouteRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var request = new GenericSearchPaginationRequest
        {
            Texto = "SCL",
            TamanhoPagina = 10,
            PaginaAtual = 1,
            CampoOrdenacao = "Valor",
            DirecaoOrdenacao = "asc"
        };

        // Act
        useCase.Execute(request);

        // Assert
        mockOutputPort.Verify(
            op => op.Ok(It.Is<GenericPaginationResponse<Route>>(response =>
                response.Total == 3)),
            Times.Once);

        mockOutputPort.Verify(op => op.NotFound(), Times.Never);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
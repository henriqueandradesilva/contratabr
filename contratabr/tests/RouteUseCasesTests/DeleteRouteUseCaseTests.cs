using Application.UseCases.V1.Route.DeleteRoute;
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

public class DeleteRouteUseCaseTests
{
    /// <summary>
    /// Remover rota
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Success_When_Route_Is_Deleted_Successfully()
    {
        // Arrange
        var routeIdSuccess = 3;

        var options = new DbContextOptionsBuilder<ContrataBRDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ContrataBRDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var route = dbContext.Set<Route>().FirstOrDefault(p => p.Id == 1);
        Assert.NotNull(route);

        var mockRouteRepository = new Mock<IRouteRepository>();
        mockRouteRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Route, bool>>>()))
            .Returns((Expression<Func<Route, bool>> predicate) =>
                dbContext.Set<Route>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Route>>();

        var useCase = new DeleteRouteUseCase(
            mockUnitOfWork.Object,
            mockRouteRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(routeIdSuccess);

        // Assert
        mockRouteRepository.Verify(repo => repo.Delete(It.IsAny<Route>()), Times.Once);
        mockOutputPort.Verify(op => op.Ok(It.IsAny<Route>()), Times.Once); // Verifica se Ok foi chamado
    }

    /// <summary>
    /// Rota não encontrada
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_NotFound_When_Route_Is_Not_Found()
    {
        // Arrange
        var routeIdNotFound = 999L;

        // Arrange
        var options = new DbContextOptionsBuilder<ContrataBRDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ContrataBRDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var mockRouteRepository = new Mock<IRouteRepository>();
        mockRouteRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Route, bool>>>()))
            .Returns((Expression<Func<Route, bool>> predicate) =>
                dbContext.Set<Route>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Route>>();

        var useCase = new DeleteRouteUseCase(
            mockUnitOfWork.Object,
            mockRouteRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(routeIdNotFound);

        // Assert
        mockOutputPort.Verify(op => op.NotFound(), Times.Once);
    }
}
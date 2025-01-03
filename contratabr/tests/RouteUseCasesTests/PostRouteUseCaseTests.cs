using Application.UseCases.V1.Route.PostRoute;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.RouteUseCasesTests;

public class PostRouteUseCaseTests
{
    //Criação de rota
    [Fact]
    public async Task Execute_Should_Return_Success_When_Route_Is_Created_Successfully()
    {
        // Arrange
        var route = new Route(
            id: 0,
            origem: "TFK1",
            destino: "TFK2",
            valor: 10.00
        );

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
        var mockOutputPort = new Mock<IOutputPort<Route>>();

        var useCase = new PostRouteUseCase(
            mockUnitOfWork.Object,
            mockRouteRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(route);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockOutputPort.Verify(op => op.Ok(route), Times.Once);
    }

    //Verificar se a rota já existe
    [Fact]
    public async Task Execute_Should_Return_Error_When_Route_Already_Exists()
    {
        // Arrange
        var route = new Route(
            id: 0,
            origem: "GRU",
            destino: "BRC",
            valor: 10.00
        );

        var options = new DbContextOptionsBuilder<ContrataBRDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ContrataBRDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var mockTaskRepository = new Mock<IRouteRepository>();
        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Route, bool>>>()))
            .Returns((Expression<Func<Route, bool>> predicate) =>
                dbContext.Set<Route>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Route>>();

        var useCase = new PostRouteUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(route);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.RouteExist), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }
}
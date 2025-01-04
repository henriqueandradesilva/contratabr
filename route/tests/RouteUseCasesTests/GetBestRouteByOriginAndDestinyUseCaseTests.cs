using Application.UseCases.V1.Route.GetRouteById;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using tests.Common;

namespace tests.RouteUseCasesTests;

public class GetBestRouteByOriginAndDestinyUseCaseTests
{
    [Fact]
    public async Task Execute_Should_Return_Success_When_BestRoute_Is_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RouteDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new RouteDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var mockRouteRepository = new Mock<IRouteRepository>();
        mockRouteRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(dbContext.Set<Route>().ToList());

        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<string>>();

        var useCase = new GetBestRouteByOriginAndDestinyUseCase(mockRouteRepository.Object, mockNotificationHelper.Object);
        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute("GRU", "CDG");

        // Assert
        mockOutputPort.Verify(op => op.Ok(It.IsAny<string>()), Times.Once);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_Route_Is_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RouteDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new RouteDbContext(options);

        await SeedMockData.Init(dbContext, true);

        var mockRouteRepository = new Mock<IRouteRepository>();
        mockRouteRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(dbContext.Set<Route>().ToList());

        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<string>>();

        var useCase = new GetBestRouteByOriginAndDestinyUseCase(mockRouteRepository.Object, mockNotificationHelper.Object);
        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute("GRU", "XYZ");

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.NotFound, MessageConst.RouteNotExist), Times.Once);
        mockOutputPort.Verify(op => op.NotFound(), Times.Once);
    }
}

using Application.UseCases.V1.Route.GetListSearchRoute.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Route.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace WebApi.UseCases.V1.Route;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rotas", Name = "Rotas")]
[ApiController]
public class GetListSearchRouteController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Route>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchRouteController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListRouteResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar rotas com paginação")]
    public IActionResult GetListSearchRoute(
       [FromServices] IGetListSearchRouteUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Route>>.Ok(
        GenericPaginationResponse<Domain.Entities.Route> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListRouteResponse>(true, new GetListRouteResponse().GetListRoute(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Route>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListRouteResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Route>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListRouteResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
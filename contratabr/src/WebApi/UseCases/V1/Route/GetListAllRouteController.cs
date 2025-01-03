using Application.UseCases.V1.Route.GetListAllRoute.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Route;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rotas", Name = "Rotas")]
[ApiController]
public class GetListAllRouteController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Route>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllRouteController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListRouteResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListRouteResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListRouteResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todas as rotas")]
    public async Task<IActionResult> GetListRoute(
        [FromServices] IGetListAllRouteUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Route>>.Ok(
        List<Domain.Entities.Route> listRoute)
        => _viewModel = base.Ok(new GenericResponse<List<GetListRouteResponse>>(true, new GetListRouteResponse().GetListRoute(listRoute), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Route>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListRouteResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Route>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListRouteResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
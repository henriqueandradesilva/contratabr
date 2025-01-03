using Application.UseCases.V1.Route.GetRouteById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Route;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rotas", Name = "Rotas")]
[ApiController]
public class GetRouteByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Route>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetRouteByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetRouteResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar uma rota por id")]
    public async Task<IActionResult> GetRoute(
        [FromServices] IGetRouteByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Route>.Ok(
        Domain.Entities.Route route)
        => _viewModel = base.Ok(new GenericResponse<GetRouteResponse>(true, new GetRouteResponse().GetRoute(route), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Route>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetRouteResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Route>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetRouteResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
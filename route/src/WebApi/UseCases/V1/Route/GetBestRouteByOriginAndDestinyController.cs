using Application.UseCases.V1.Route.GetRouteById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
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
public class GetBestRouteByOriginAndDestinyController : CustomControllerBaseExtension, IOutputPortWithNotFound<string>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetBestRouteByOriginAndDestinyController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("melhor/origem/{origem}/destino/{destino}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<string>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar a melhor rota por origem e destino")]
    public async Task<IActionResult> GetRoute(
        [FromServices] IGetBestRouteByOriginAndDestinyUseCase useCase,
        [FromRoute][Required] string origem,
        [FromRoute][Required] string destino)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(origem, destino);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<string>.Ok(
        string route)
        => _viewModel = base.Ok(new GenericResponse<string>(true, route, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<string>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<string>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<string>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<string>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
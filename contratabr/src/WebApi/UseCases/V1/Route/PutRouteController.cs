using Application.UseCases.V1.Route.PutRoute.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Route.Request;
using CrossCutting.Dtos.Route.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Route;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rotas", Name = "Rotas")]
[ApiController]
public class PutRouteController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Route>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutRouteController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutRouteResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar uma rota por id")]
    public async Task<IActionResult> PutRoute(
        [FromServices] IPutRouteUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutRouteRequest putRouteRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Route route =
            new Domain.Entities.Route(
            id,
            putRouteRequest.Origem,
            putRouteRequest.Destino,
            putRouteRequest.Valor);

        await useCase.Execute(route);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Route>.Ok(
        Domain.Entities.Route route)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.RouteUpdated);

        _viewModel = base.Ok(new GenericResponse<PutRouteResponse>(true, new PutRouteResponse(route), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Route>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutRouteResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}
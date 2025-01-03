using Application.UseCases.V1.Route.PostRoute.Interfaces;
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
public class PostRouteController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Route>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostRouteController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostRouteResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostRouteResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar uma nova rota")]
    public async Task<IActionResult> PostRoute(
        [FromServices] IPostRouteUseCase useCase,
        [FromBody][Required] PostRouteRequest postRouteRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Route project =
            new Domain.Entities.Route(
            0,
            postRouteRequest.Origem,
            postRouteRequest.Destino,
            postRouteRequest.Valor);

        await useCase.Execute(project);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Route>.Ok(
        Domain.Entities.Route route)
    {
        var uri = $"/rotas/{route.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.RouteCreated);

        var response =
            new GenericResponse<PostRouteResponse>(true, new PostRouteResponse(route), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Route>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostRouteResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}
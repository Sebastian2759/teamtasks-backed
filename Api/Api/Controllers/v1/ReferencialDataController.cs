using Application.UseCases.ReferencialData.GetReferencialDataById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Enums.Enums;
using Constants = Application.Constants.Constants;

namespace Api.Controllers.v1;

/// <summary>
/// Controlador para manejar las solicitudes de datos referenciales.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize]
public class ReferencialDataController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ReferencialDataController"/> con el mediador y la configuración necesarios.
    /// </summary>
    /// <param name="mediator">El mediador a utilizar para enviar solicitudes de comandos y consultas, siguiendo el patrón CQRS.</param>
    /// <param name="configuration">La configuración de la aplicación para acceder a los valores de configuración.</param>
    public ReferencialDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Método general para obtener datos referenciales por tipo.
    /// </summary>
    /// <param name="type">El tipo de dato referencial a obtener.</param>
    /// <returns>Una acción de resultado que contiene los datos referenciales solicitados.</returns>
    private async Task<IActionResult> GetReferentialData(ReferentialDataType type)
    {
        if (!Constants.IdsReferencesData.TryGetValue(type, out Guid id))
            return NotFound($"{type} not found.");

        var request = new GetReferencialDataByIdRequest { Id = id };
        var result = await _mediator.Send(request);

        var details = result.Data.referencialDataDetails
            .OrderBy(x => x.Description);

        return Ok(new GetReferencialDataByIdResponse { referencialDataDetails = details });
    }

    [HttpGet("state-proyect")]
    public Task<IActionResult> GetStateProyect() =>
    GetReferentialData(ReferentialDataType.StateProyect);

    [HttpGet("state-area")]
    public Task<IActionResult> GetStateArea() =>
        GetReferentialData(ReferentialDataType.StateArea);

    [HttpGet("task-statuses")]
    public Task<IActionResult> GetTaskStatuses() =>
        GetReferentialData(ReferentialDataType.StateArea);

    [HttpGet("task-priorities")]
    public Task<IActionResult> GetTaskPriorities() =>
        GetReferentialData(ReferentialDataType.PropertiesTask);

}
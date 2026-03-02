using CargoManagement.Api.DTOs;
using CargoManagement.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CargoManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargosController : ControllerBase
{
    private readonly ICargoService _cargoService;
    private readonly IValidator<CreateCargoDto> _validator;
    private readonly ILogger<CargosController> _logger;

    public CargosController(
        ICargoService cargoService,
        IValidator<CreateCargoDto> validator,
        ILogger<CargosController> logger)
    {
        _cargoService = cargoService;
        _validator = validator;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as cargas com paginação.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<CargoDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _cargoService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Obtém uma carga pelo ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cargo = await _cargoService.GetByIdAsync(id);
        if (cargo is null)
            return NotFound(new { message = $"Carga com ID {id} não encontrada." });

        return Ok(cargo);
    }

    /// <summary>
    /// Busca cargas por período usando stored procedure.
    /// </summary>
    [HttpGet("por-periodo")]
    public async Task<IActionResult> GetByPeriod(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        if (dataInicio > dataFim)
            return BadRequest(new { message = "Data de início deve ser anterior à data de fim." });

        var result = await _cargoService.GetByPeriodAsync(dataInicio, dataFim);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova carga.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCargoDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var cargo = await _cargoService.CreateAsync(dto);
        _logger.LogInformation("Carga {Numero} criada com ID {Id}", cargo.Numero, cargo.Id);

        return CreatedAtAction(nameof(GetById), new { id = cargo.Id }, cargo);
    }

    /// <summary>
    /// Atualiza uma carga existente.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCargoDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var cargo = await _cargoService.UpdateAsync(id, dto);
        if (cargo is null)
            return NotFound(new { message = $"Carga com ID {id} não encontrada." });

        return Ok(cargo);
    }

    /// <summary>
    /// Remove uma carga.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _cargoService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Carga com ID {id} não encontrada." });

        return NoContent();
    }
}

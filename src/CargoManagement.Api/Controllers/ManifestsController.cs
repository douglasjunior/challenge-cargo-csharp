using CargoManagement.Api.DTOs;
using CargoManagement.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CargoManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManifestsController : ControllerBase
{
    private readonly IManifestService _manifestService;
    private readonly IValidator<CreateManifestDto> _validator;
    private readonly ILogger<ManifestsController> _logger;

    public ManifestsController(
        IManifestService manifestService,
        IValidator<CreateManifestDto> validator,
        ILogger<ManifestsController> logger)
    {
        _manifestService = manifestService;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var manifests = await _manifestService.GetAllAsync();
        return Ok(manifests);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manifest = await _manifestService.GetByIdAsync(id);
        if (manifest is null)
            return NotFound(new { message = $"Manifesto com ID {id} não encontrado." });

        return Ok(manifest);
    }

    [HttpGet("por-carga/{cargoId:int}")]
    public async Task<IActionResult> GetByCargoId(int cargoId)
    {
        var manifests = await _manifestService.GetByCargoIdAsync(cargoId);
        return Ok(manifests);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateManifestDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var manifest = await _manifestService.CreateAsync(dto);
        _logger.LogInformation("Manifesto {Numero} criado com ID {Id}", manifest.Numero, manifest.Id);

        return CreatedAtAction(nameof(GetById), new { id = manifest.Id }, manifest);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateManifestDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var manifest = await _manifestService.UpdateAsync(id, dto);
        if (manifest is null)
            return NotFound(new { message = $"Manifesto com ID {id} não encontrado." });

        return Ok(manifest);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _manifestService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Manifesto com ID {id} não encontrado." });

        return NoContent();
    }
}

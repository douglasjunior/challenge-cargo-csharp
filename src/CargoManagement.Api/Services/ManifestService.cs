using CargoManagement.Api.DTOs;
using CargoManagement.Api.Models;
using CargoManagement.Api.Repositories.Interfaces;
using CargoManagement.Api.Services.Interfaces;

namespace CargoManagement.Api.Services;

public class ManifestService : IManifestService
{
    private readonly IManifestRepository _manifestRepository;

    public ManifestService(IManifestRepository manifestRepository)
    {
        _manifestRepository = manifestRepository;
    }

    public async Task<IEnumerable<Manifest>> GetAllAsync()
    {
        return await _manifestRepository.GetAllAsync();
    }

    public async Task<Manifest?> GetByIdAsync(int id)
    {
        return await _manifestRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Manifest>> GetByCargoIdAsync(int cargoId)
    {
        return await _manifestRepository.GetByCargoIdAsync(cargoId);
    }

    public async Task<Manifest> CreateAsync(CreateManifestDto dto)
    {
        var manifest = new Manifest
        {
            CargoId = dto.CargoId,
            Numero = dto.Numero,
            DataEmissao = dto.DataEmissao,
            Despachante = dto.Despachante,
            Observacoes = dto.Observacoes
        };

        return await _manifestRepository.CreateAsync(manifest);
    }

    public async Task<Manifest?> UpdateAsync(int id, CreateManifestDto dto)
    {
        var manifest = await _manifestRepository.GetByIdAsync(id);
        if (manifest is null) return null;

        manifest.CargoId = dto.CargoId;
        manifest.Numero = dto.Numero;
        manifest.DataEmissao = dto.DataEmissao;
        manifest.Despachante = dto.Despachante;
        manifest.Observacoes = dto.Observacoes;

        return await _manifestRepository.UpdateAsync(manifest);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _manifestRepository.DeleteAsync(id);
    }
}

using CargoManagement.Api.Data;
using CargoManagement.Api.Models;
using CargoManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoManagement.Api.Repositories;

public class ManifestRepository : IManifestRepository
{
    private readonly CargoDbContext _context;

    public ManifestRepository(CargoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Manifest>> GetAllAsync()
    {
        return await _context.Manifests
            .Include(m => m.Cargo)
            .OrderByDescending(m => m.DataEmissao)
            .ToListAsync();
    }

    public async Task<Manifest?> GetByIdAsync(int id)
    {
        return await _context.Manifests
            .Include(m => m.Cargo)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Manifest>> GetByCargoIdAsync(int cargoId)
    {
        return await _context.Manifests
            .Include(m => m.Cargo)
            .Where(m => m.CargoId == cargoId)
            .OrderByDescending(m => m.DataEmissao)
            .ToListAsync();
    }

    public async Task<Manifest> CreateAsync(Manifest manifest)
    {
        _context.Manifests.Add(manifest);
        await _context.SaveChangesAsync();
        return manifest;
    }

    public async Task<Manifest> UpdateAsync(Manifest manifest)
    {
        _context.Manifests.Update(manifest);
        await _context.SaveChangesAsync();
        return manifest;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var manifest = await _context.Manifests.FindAsync(id);
        if (manifest is null) return false;

        _context.Manifests.Remove(manifest);
        await _context.SaveChangesAsync();
        return true;
    }
}

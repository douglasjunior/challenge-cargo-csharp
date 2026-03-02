using CargoManagement.Api.Data;
using CargoManagement.Api.Models;
using CargoManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoManagement.Api.Repositories;

public class CargoRepository : ICargoRepository
{
    private readonly CargoDbContext _context;

    public CargoRepository(CargoDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Cargo> Items, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        var totalCount = await _context.Cargos.CountAsync();

        var items = await _context.Cargos
            .Include(c => c.Containers)
            .OrderByDescending(c => c.DataRegistro)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Cargo?> GetByIdAsync(int id)
    {
        return await _context.Cargos
            .Include(c => c.Containers)
            .Include(c => c.Manifests)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cargo> CreateAsync(Cargo cargo)
    {
        _context.Cargos.Add(cargo);
        await _context.SaveChangesAsync();
        return cargo;
    }

    public async Task<Cargo> UpdateAsync(Cargo cargo)
    {
        _context.Cargos.Update(cargo);
        await _context.SaveChangesAsync();
        return cargo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cargo = await _context.Cargos.FindAsync(id);
        if (cargo is null) return false;

        _context.Cargos.Remove(cargo);
        await _context.SaveChangesAsync();
        return true;
    }
}

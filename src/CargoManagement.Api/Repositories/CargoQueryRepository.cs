using CargoManagement.Api.DTOs;
using CargoManagement.Api.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace CargoManagement.Api.Repositories;

public class CargoQueryRepository : ICargoQueryRepository
{
    private readonly string _connectionString;

    public CargoQueryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<CargoDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var result = await connection.QueryAsync<CargoDto>(
            "SELECT * FROM buscar_cargas_por_periodo(@data_inicio, @data_fim)",
            new { data_inicio = startDate, data_fim = endDate });

        return result;
    }
}

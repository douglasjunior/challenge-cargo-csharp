namespace CargoManagement.Api.DTOs;

public class ManifestDto
{
    public int Id { get; set; }
    public int CargoId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public string Despachante { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}

public class CreateManifestDto
{
    public int CargoId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public string Despachante { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}

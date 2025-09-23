namespace VisionHive.Application.DTO.Request;

/// <summary>
/// Parâmetros de paginação, busca e ordenação para listar <c>Patio</c>
/// Este DTO é consumido pelo endpoint GET /api/v1/patios
/// </summary>
public sealed class PatioPaginatedRequest
{
    /// <summary>
    /// Número da página (>=1)
    /// <example>1</example>
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Tamanho da página (1..100)
    /// <example>10</example>
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Texto de busca (aplica em Nome)
    /// <example>Central</example>
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Campo para ordenação (nome ou limiteMotos)
    /// <example>nome</example>
    /// </summary>
    public string? SortBy { get; set; } = "nome";
    
    /// <summary>
    /// Direção da ordenação (Asc ou Desc)
    /// <example>Asc</example>
    /// </summary>
    public string? SortDir { get; set; } = "Asc";

}
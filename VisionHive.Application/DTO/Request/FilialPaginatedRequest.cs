namespace VisionHive.Application.DTO.Request;

/// <summary>
/// Parâmetros de paginação, busca e ordenação para listar <c>Filial</c>.
/// Usado pelo endpoint GET /api/v1/filiais.
/// </summary>
public sealed class FilialPaginatedRequest
{
    /// <summary>Número da página (>= 1).</summary>
    /// <example>1</example>
    public int PageNumber { get; set; } = 1;

    /// <summary>Tamanho da página (1..100).</summary>
    /// <example>10</example>
    public int PageSize { get; set; } = 10;

    /// <summary>Texto de busca (aplica em Nome, Bairro ou Cnpj).</summary>
    /// <example>Centro</example>
    public string? Search { get; set; }

    /// <summary>Campo para ordenação (nome, bairro ou cnpj).</summary>
    /// <example>nome</example>
    public string? SortBy { get; set; } = "nome";

    /// <summary>Direção da ordenação (Asc ou Desc).</summary>
    /// <example>Asc</example>
    public string? SortDir { get; set; } = "Asc";
}
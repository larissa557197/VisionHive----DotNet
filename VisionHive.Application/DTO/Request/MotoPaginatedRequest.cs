namespace VisionHive.Application.DTO.Request;

// parametros para paginação e filtro na listagem de motos
public sealed class MotoPaginatedRequest
{
    // número da página (>=1)
    public int PageNumber { get; set; } = 1;
    
    // quantidade de itens por página (1 - 100)
    public int PageSize { get; set; } = 10; 
    
    // texto de busca(placa, chassi ou número do motor)
    public string? Search { get; set; }
    
    // campo para ordenação (id, placa, chassi, numeroMotor, prioridade)
    public string? SortBy { get; set; } = "id"; 
    
    // direção da ordenação (Asc ou Desc)
    public string? SortDir { get; set; } = "Desc"; 
}
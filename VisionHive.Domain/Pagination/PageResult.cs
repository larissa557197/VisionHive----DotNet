namespace VisionHive.Domain.Pagination;

// estrutura de resultado paginado genérica
// usada para devolver coleções com metados de paginação
// 'T' = tipo de entidade ou DTO retornado
public class PageResult<T>
{
    // itens da pagiina atual
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    
    // número da página atual
    public int Page { get; set; }
    
    // quantidade de itens por página
    public int PageSize { get; set; }
    
    // total de registros encontrados
    public long Total { get; set; }
    
    // Indica se existe próxima página
    public bool HasNext => Page < TotalPages;
    
    // indica se existe uma página anterior
    public bool HasPrevious => Page > 1;
    
    // total de páginas calculado
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);


}
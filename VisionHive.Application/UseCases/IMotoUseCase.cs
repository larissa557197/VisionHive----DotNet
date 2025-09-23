using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Application.UseCases;

// contrato de operações da entidade moto
// define métodos de CRUD e paginação para uso pelos controllers
public interface IMotoUseCase
{
    // retorna lista paginada de motos com filtros e ordenação
    Task<PageResult<Moto>> GetPagination(MotoPaginatedRequest request);
    
    /// busca uma moto específica pelo ID
    Task<Moto?> GetByIdAsync(Guid id);
    
    Task<Moto?> CreateAsync(MotoRequest request);
    
    // atualiza uma moto existente
    Task<bool> UpdateAsync(Guid id, MotoRequest request);
    
    // remove uma moto pelo seu ID
    Task<bool> DeleteAsync(Guid id);
}
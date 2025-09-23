using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Application.UseCases;

/// <summary>
/// Contrato de operações disponíveis para a entidade <see cref="Patio"/>
/// Define os métodos que os Controllers irão consumir
/// </summary>
public interface IPatioUseCase
{
    /// <summary>
    /// Retorna uma lista paginada de pátios, com suporte a busca e ordenação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação e filtro.</param>
    /// <returns>Objeto <see cref="PageResult{T}"/> contendo os itens e metadados.</returns>
    Task<PageResult<Patio>> GetPagination(PatioPaginatedRequest request);
    
    /// <summary>
    /// Busca um pátio específico pelo seu identificador.
    /// </summary>
    /// <param name="id">ID do pátio.</param>
    /// <returns>Instância de <see cref="Patio"/> ou <c>null</c> se não encontrada.</returns>
    Task<Patio?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Cria um novo pátio a partir do DTO de entrada.
    /// </summary>
    /// <param name="request">Dados para criação do pátio.</param>
    /// <returns>Pátio criado.</returns>
    Task<Patio> CreateAsync(PatioRequest request);

    /// <summary>
    /// Atualiza um pátio existente.
    /// </summary>
    /// <param name="id">ID do pátio a ser atualizado.</param>
    /// <param name="request">Novos dados do pátio.</param>
    /// <returns><c>true</c> se atualizou; <c>false</c> se não encontrado.</returns>
    Task<bool> UpdateAsync(Guid id, PatioRequest request);

    /// <summary>
    /// Remove um pátio pelo seu identificador.
    /// </summary>
    /// <param name="id">ID do pátio.</param>
    /// <returns><c>true</c> se removeu; <c>false</c> se não encontrado.</returns>
    Task<bool> DeleteAsync(Guid id);
}
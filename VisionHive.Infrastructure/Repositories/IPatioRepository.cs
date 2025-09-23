using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Infrastructure.Repositories;

/// <summary>
/// Contrato de acesso a dados para a entidade <see cref="Patio"/>
/// Define operações de CRUD e listagem paginada com filtro/ordenação
/// </summary>
public interface IPatioRepository
{
    /// <summary>
    /// Adiciona um novo <see cref="Patio"/> ao banco de dados
    /// <param name="patio">Instância a ser persistida</param>
    /// <param name="ct">Token de cancelamento (opcional)</param>
    /// <returns>Entidade persistida (com <c>Id</c> preenchido)</returns>
    /// </summary>
    Task<Patio> AddAsync(Patio patio, CancellationToken ct = default);
    
    /// <summary>
    /// Retorna uma página de <see cref="Patio"/> com suporte a busca por nome.
    /// </summary>
    /// <param name="page">Número da página (>= 1).</param>
    /// <param name="pageSize">Tamanho da página (>= 1).</param>
    /// <param name="search">Texto para busca no <c>Nome</c> (opcional).</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns>
    /// Um <see cref="PageResult{T}"/> com <c>Items</c>, <c>Page</c>, <c>PageSize</c>, <c>Total</c>
    /// e auxiliares de navegação.
    /// </returns>
    Task<PageResult<Patio>> GetPaginationAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default
        );

    /// <summary>
    /// Obtém um <see cref="Patio"/> pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do pátio.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns>Entidade encontrada ou <c>null</c>.</returns>
    Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default);
    
    
    /// <summary>
    /// Atualiza os dados de um <see cref="Patio"/>.
    /// </summary>
    /// <param name="patio">Instância com os novos valores.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns><c>true</c> se ao menos um registro foi afetado; caso contrário, <c>false</c>.</returns>
    Task<bool> UpdateAsync(Patio patio, CancellationToken ct = default);
    
    /// <summary>
    /// Remove um <see cref="Patio"/> pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do pátio.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns><c>true</c> se excluiu; <c>false</c> se não encontrado.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
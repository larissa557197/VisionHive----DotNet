using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Infrastructure.Repositories;

/// <summary>
/// Contrato de acesso a dados para a entidade <see cref="Filial"/>.
/// Define operações de CRUD e listagem paginada com filtro/ordenação.
/// </summary>
public interface IFilialRepository
{
    /// <summary>
    /// Adiciona uma nova <see cref="Filial"/> ao banco de dados.
    /// </summary>
    /// <param name="filial">Instância a ser persistida.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns>Entidade persistida (com <c>Id</c> preenchido).</returns>
    Task<Filial> AddAsync(Filial filial, CancellationToken ct = default);


    /// <summary>
    /// Retorna uma página de <see cref="Filial"/> com suporte a busca por nome, bairro ou CNPJ.
    /// </summary>
    /// <param name="page">Número da página (>= 1).</param>
    /// <param name="pageSize">Tamanho da página (>= 1).</param>
    /// <param name="search">Texto para busca (aplica em Nome, Bairro e Cnpj).</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns>
    /// Um <see cref="PageResult{T}"/> com <c>Items</c>, <c>Page</c>, <c>PageSize</c>, <c>Total</c>
    /// e auxiliares de navegação.
    /// </returns>
    Task<PageResult<Filial>> GetPaginationAsync(
        int page, 
        int pageSize, 
        string? search, 
        CancellationToken ct = default
        );
    
    
    /// <summary>
    /// Obtém uma <see cref="Filial"/> pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da filial.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns>Entidade encontrada ou <c>null</c>.</returns>
    Task<Filial> GetByIdAsync(Guid id, CancellationToken ct = default);
    
    
    /// <summary>
    /// Atualiza os dados de uma <see cref="Filial"/>.
    /// </summary>
    /// <param name="filial">Instância com os novos valores.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns><c>true</c> se ao menos um registro foi afetado; caso contrário, <c>false</c>.</returns>
    Task<bool> UpdateAsync(Filial filial, CancellationToken ct = default);


    /// <summary>
    /// Remove uma <see cref="Filial"/> pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da filial.</param>
    /// <param name="ct">Token de cancelamento (opcional).</param>
    /// <returns><c>true</c> se excluiu; <c>false</c> se não encontrado.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
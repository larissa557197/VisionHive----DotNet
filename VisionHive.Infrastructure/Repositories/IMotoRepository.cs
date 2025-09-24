
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Infrastructure.Repositories;

// contrato de acesso a dados para Moto
// define operações de CRUD e paginação
public interface IMotoRepository
{
  /// <summary>
  /// Adiciona uma nova moto ao banco de dados.
  /// </summary>
  /// <param name="moto">Instância da entidade <see cref="Moto"/> a ser persistida.</param>
  /// <param name="ct">Token de cancelamento (opcional).</param>
  /// <returns>A entidade persistida, já com Id preenchido.</returns>
  Task<Moto> AddAsync(Moto moto, CancellationToken ct = default);

  /// <summary>
  /// Retorna uma lista paginada de motos, com suporte a filtro de busca.
  /// </summary>
  /// <param name="page">Número da página (>= 1).</param>
  /// <param name="pageSize">Tamanho da página (>= 1).</param>
  /// <param name="search">Texto de busca (placa, chassi ou número do motor).</param>
  /// <param name="ct">Token de cancelamento (opcional).</param>
  /// <returns>Objeto <see cref="PageResult{T}"/> contendo a página de resultados.</returns>
  Task<PageResult<Moto>> GetPaginationAsync(
      int page,
      int pageSize,
      string? search,
      CancellationToken ct = default);

  /// <summary>
  /// Busca uma moto pelo identificador único.
  /// </summary>
  /// <param name="id">Id da moto.</param>
  /// <param name="ct">Token de cancelamento (opcional).</param>
  /// <returns>A moto encontrada ou null.</returns>
  Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default);

  /// <summary>
  /// Atualiza uma moto existente.
  /// </summary>
  /// <param name="moto">Instância da moto com os novos dados.</param>
  /// <param name="ct">Token de cancelamento (opcional).</param>
  /// <returns>True se atualizou pelo menos um registro, senão false.</returns>
  Task<bool> UpdateAsync(Moto moto, CancellationToken ct = default);

  /// <summary>
  /// Exclui uma moto pelo seu identificador.
  /// </summary>
  /// <param name="id">Id da moto.</param>
  /// <param name="ct">Token de cancelamento (opcional).</param>
  /// <returns>True se excluiu, false se não encontrou.</returns>
  Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Application.UseCases
{

    /// <summary>
    /// Contrato de operações disponíveis para a entidade <see cref="Filial"/>.
    /// Define os métodos que os Controllers irão consumir.
    /// </summary>
    public interface IFilialUseCase
    {
        /// <summary>
        /// Retorna uma lista paginada de filiais, com suporte a busca e ordenação.
        /// </summary>
        /// <param name="request">Parâmetros de paginação/filtro.</param>
        /// <returns>Um <see cref="PageResult{T}"/> contendo itens e metadados.</returns>
        Task<PageResult<Filial>> GetPagination(FilialPaginatedRequest request);


        /// <summary>
        /// Busca uma filial específica pelo seu identificador.
        /// </summary>
        /// <param name="id">ID da filial.</param>
        /// <returns>Instância de <see cref="Filial"/> ou <c>null</c> se não encontrada.</returns>
        Task<Filial?> GetByIdAsync(Guid id);
        
        
        /// <summary>
        /// Cria uma nova filial.
        /// </summary>
        /// <param name="request">Dados de criação.</param>
        /// <returns>Entidade criada.</returns>
        Task<Filial> CreateAsync(FilialRequest request);


        /// <summary>
        /// Atualiza uma filial existente.
        /// </summary>
        /// <param name="id">ID da filial a ser atualizada.</param>
        /// <param name="request">Novos dados da filial.</param>
        /// <returns><c>true</c> se atualizou; <c>false</c> se não encontrada.</returns>
        Task<bool> UpdateAsync(Guid id, FilialRequest request);


        /// <summary>
        /// Remove uma filial pelo seu identificador.
        /// </summary>
        /// <param name="id">ID da filial.</param>
        /// <returns><c>true</c> se removeu; <c>false</c> se não encontrada.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}

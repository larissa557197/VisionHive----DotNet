using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.Application.UseCases
{
    /// <summary>
    /// Implementação do caso de uso da entidade <see cref="Filial"/>.
    /// Centraliza regras de negócio e delega persistência ao <see cref="IFilialRepository"/>.
    /// </summary>
    public sealed class FilialUseCase(IFilialRepository filialRepository) : IFilialUseCase
    {
        public async Task<PageResult<Filial>> GetPagination(FilialPaginatedRequest request)
        {
            // normaliza parâmetros de paginação para evitar valores inválidos
            var page = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

           return await filialRepository.GetPaginationAsync(page, pageSize, request.Search);
        }

        public async Task<Filial?> GetByIdAsync(Guid id)
        {
            return await filialRepository.GetByIdAsync(id);
        }
        
        public async Task<Filial> CreateAsync(FilialRequest request)
        { 
            // regras de entrada
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ArgumentException("O nome da filial não pode ser vazio.", nameof(request.Nome));

            if (string.IsNullOrWhiteSpace(request.Bairro))
                throw new ArgumentException("O bairro da filial não pode ser vazio.", nameof(request.Bairro));

            if (string.IsNullOrWhiteSpace(request.Cnpj))
                throw new ArgumentException("O CNPJ da filial não pode ser vazio.", nameof(request.Cnpj));
            
            // instancia a entidade de dominio
            var entity = new Filial(
                nome:  request.Nome,
                bairro:  request.Bairro,
                cnpj:   request.Cnpj
                );
            
            // persiste via repositório
            return await filialRepository.AddAsync(entity);

        }

        public async Task<bool> UpdateAsync(Guid id, FilialRequest request)
        {
            // revalida entrada
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ArgumentException("O nome da filial não pode ser vazio.", nameof(request.Nome));

            if (string.IsNullOrWhiteSpace(request.Bairro))
                throw new ArgumentException("O bairro da filial não pode ser vazio.", nameof(request.Bairro));

            if (string.IsNullOrWhiteSpace(request.Cnpj))
                throw new ArgumentException("O CNPJ da filial não pode ser vazio.", nameof(request.Cnpj));
            
            // busca a entidade atual
            var entity = await filialRepository.GetByIdAsync(id);
            if(entity is null) return false;
            
            // aplica atualização
            entity.AtualizarDados(
                nome: request.Nome,
                bairro: request.Bairro,
                cnpj: request.Cnpj
                );
            
            // persiste alterações
            return await filialRepository.UpdateAsync(entity);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            return await filialRepository.DeleteAsync(id);
        }
        
    }
}

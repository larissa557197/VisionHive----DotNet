using Microsoft.EntityFrameworkCore.Query.Internal;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.Application.UseCases;

/// <summary>
/// Implementação do caso de uso da entidade <see cref="Patio"/>
/// Centraliza regras de negócio e delega persistência ao <see cref="IPatioRepository"/>
/// </summary>
public sealed class PatioUseCase(IPatioRepository  patioRepository) : IPatioUseCase
{
    public async Task<PageResult<Patio>> GetPagination(PatioPaginatedRequest request)
    {
        // normaliza paginação (evita números inválidos)
        var page = request.PageNumber < 1 ? 1  : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);
        
        // OBS: neste padrão, a ordenação é aplicada no repositório (por Nome ASC)
        return await patioRepository.GetPaginationAsync(page, pageSize, request.Search);
    }

    public async Task<Patio?> GetByIdAsync(Guid id)
    {
        return await patioRepository.GetByIdAsync(id);
    }

    public async Task<Patio> CreateAsync(PatioRequest request)
    {
        // validações de entrada 
        if(string.IsNullOrWhiteSpace(request.Nome)) 
            throw new ArgumentException("O nome do pátio não pode ser vazio", nameof(request.Nome));

        if (request.LimiteMotos <= 0)
            throw new ArgumentException("O limite de motos deve ser maior que zero", nameof(request.LimiteMotos));
        
        // cria a entidade de domínio 
        var entity = new Patio(
            nome: request.Nome,
            limiteMotos: request.LimiteMotos,
            filialId: request.FilialId
            );
        
        // persiste via repositório
        return await patioRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, PatioRequest request)
    {
        // regras de entrada
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("O nome do pátio não pode ser vazio", nameof(request.Nome));

        if (request.LimiteMotos <= 0)
            throw new ArgumentException("O limite de motos deve ser maior que zero", nameof(request.LimiteMotos));
        
        // busca entidade atual
        var entity = await patioRepository.GetByIdAsync(id);
        if (entity == null) return false;
        
        // aplica atualização (o método também valida internamente)
        entity.AtualizarDados(
            nome:  request.Nome,
            limiteMotos:  request.LimiteMotos
            );
        
        // persiste alterações
        return await patioRepository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await patioRepository.DeleteAsync(id);
    }
}
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Repositories;
using VisionHive.Domain.Enums;

namespace VisionHive.Application.UseCases;

/// <summary>
/// Implementação do caso de uso da entidade <see cref="Moto"/>
/// Orquestra as regras de negócio e delega o acesso a dados para o <see cref="IMotoRepository"/>
/// </summary>
public class MotoUseCase(IMotoRepository motoRepository) : IMotoUseCase
{
    public async Task<PageResult<Moto>> GetPagination(MotoPaginatedRequest request)
    {
        var page = request.PageNumber < 1 ?  1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);
        
        // usa o repositório para buscar com paginação
        return await motoRepository.GetPaginationAsync(page, pageSize, request.Search);
        
    }

    public async Task<Moto?> GetByIdAsync(Guid id)
    {
        return await motoRepository.GetByIdAsync(id);
    }
    

    public async Task<Moto> CreateAsync(MotoRequest request)
    {
        // regra: precisa ter pelo menos um identificador
        if (string.IsNullOrWhiteSpace(request.Placa) &&
            string.IsNullOrWhiteSpace(request.Chassi) &&
            string.IsNullOrWhiteSpace(request.NumeroMotor))
            throw new ArgumentException("Informe pelo menos um identificador (Placa | Chassi | Número do Motor");
        
        // cria entidade do dominio
        var entity = new Moto(
            placa: request.Placa,
            chassi: request.Chassi,
            numeroMotor: request.NumeroMotor,
            prioridade: request.Prioridade,
            patioId: request.PatioId
        );
        
        // persiste via repositório
        return await motoRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, MotoRequest request)
    {
        // revalida entrada
        if (string.IsNullOrWhiteSpace(request.Placa) &&
            string.IsNullOrWhiteSpace(request.Chassi) &&
            string.IsNullOrWhiteSpace(request.NumeroMotor))
            throw new Exception("Informe pelo menos Placa, Chassi ou Número do Motor");
        
        // busca entidade existente
        var entity = await motoRepository.GetByIdAsync(id);
        if (entity == null) return false;
        
        // aplica atualização de dados conforme a regra do dominio
        entity.AtualizarDados(
            placa: request.Placa,
            chassi: request.Chassi,
            numeroMotor: request.NumeroMotor,
            prioridade: request.Prioridade,
            patioId: request.PatioId
            );
        
        // persiste mudanças
        return await motoRepository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await motoRepository.DeleteAsync(id);
    }
}
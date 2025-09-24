using Microsoft.EntityFrameworkCore;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IMotoRepository"/> usando EF Core
/// Responsável por persistir e recuperar dados da entidade <see cref="Moto"/>
/// </summary>
public sealed class MotoRepository(VisionHiveContext context) : IMotoRepository
{
    public async Task<Moto> AddAsync(Moto moto, CancellationToken ct = default)
    {
        // adiciona a moto ao contexto e salva no banco
        context.Add((moto));
        await context.SaveChangesAsync(ct);
        return moto;
    }

    public async Task<PageResult<Moto>> GetPaginationAsync(
        int page, 
        int pageSize, 
        string? search, 
        CancellationToken ct = default
        )
    {
        // query inicial: todas as motos, sem tracking (somente leitura)
        var query = context.Motos
            .Include(m => m.Patio)
            .AsNoTracking();
        
        // se informou termo de busca, filtra por placa/chassi/número do motor
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(m =>
                (m.Placa ?? string.Empty).Contains(term) ||
                (m.Chassi ?? string.Empty).Contains(term) ||
                (m.NumeroMotor ?? string.Empty).Contains(term));
        }
        
        // ordena por ID desc
        query = query.OrderByDescending(m => m.Id);
        
        // conta total de registros antes da paginação
        var total = await query.LongCountAsync(ct);
        
        // aplica paginação (Skip/Take)
        IReadOnlyList<Moto> items = await query
            .Skip((page -1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        // retorna PageResult pronto
        return new PageResult<Moto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // busca por chave primária
        return await context.Motos
            .AsNoTracking()
            .Include(m => m.Patio)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(Moto moto, CancellationToken ct = default)
    {
        // anexa a entidade ao contexto e marca como modificada
        context.Attach(moto);
        context.Entry(moto).State = EntityState.Modified;
        
        // salva alterações
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        // busca a moto a ser deletada
        var entity = await context.Motos.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (entity == null) return false;
        
        // remove e salva
        context.Motos.Remove(entity);
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
    }
}
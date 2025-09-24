using Microsoft.EntityFrameworkCore;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IPatioRepository"/> usando EF Core
/// Responsável por persistir e consultar dados da entidade <see cref="Patio"/>
/// </summary>
public sealed class PatioRepository(VisionHiveContext context) : IPatioRepository
{
    public async Task<Patio> AddAsync(Patio patio, CancellationToken ct = default)
    {
        // adiciona a entidade ao contexto e salva no banco 
        context.Add(patio);
        await context.SaveChangesAsync(ct);
        return patio;
    }

    public async Task<PageResult<Patio>> GetPaginationAsync(
        int page, 
        int pageSize, 
        string? search, 
        CancellationToken ct = default)
    {
        // query base: leitura sem rastreamento melhora performance
        IQueryable<Patio> query = context.Patios
            .AsNoTracking()
            .Include(p => p.Filial)
            .Include(p => p.Motos);
        
        // filtro por nome, se informado
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p => (p.Nome ?? string.Empty).Contains(term));
        }
        
        // ordenação padrão: Nome ASC, depois Id (para ordenação estável)
        query = query
            .OrderBy(p => p.Nome)
            .ThenBy(p => p.Id);
        
        // total de registros antes da paginação
        var total = await query.LongCountAsync(ct);
        
        // página solicitada: Skip/Take
        IReadOnlyList<Patio> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PageResult<Patio>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public async Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // busca por Id 
        return await context.Patios
            .AsNoTracking()
            .Include(p => p.Filial)
            .Include(p => p.Motos)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(Patio patio, CancellationToken ct = default)
    {
        // anexa a entidade e marca como modificada para parsistir as alterações
        context.Attach(patio);
        context.Entry(patio).State = EntityState.Modified;
        
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        // localiza a entidade, se não existir, retorna false
        var entity = await context.Patios.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (entity == null) return false;
        
        // remove e salva
        context.Patios.Remove(entity);
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
    }
}
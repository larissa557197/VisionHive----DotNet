using Microsoft.EntityFrameworkCore;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.Infrastructure.Repositories;

/// <summary>
/// Implementação de <see cref="IFilialRepository"/> usando EF Core.
/// Responsável por persistir e consultar dados da entidade <see cref="Filial"/>.
/// </summary>
public sealed class FilialRepository(VisionHiveContext context) : IFilialRepository
{
    public async Task<Filial> AddAsync(Filial filial, CancellationToken ct = default)
    {
        // adiciona a entidade ao contexto e salva no banco
        context.Add(filial);
        await context.SaveChangesAsync(ct);
        return filial;
    }

    public async Task<PageResult<Filial>> GetPaginationAsync(
        int page, 
        int pageSize, 
        string? search, 
        CancellationToken ct = default)
    {
        
        // query base: leitura sem rastreamento
        IQueryable<Filial> query = context.Filiais
            .AsNoTracking()
            .Include(f => f.Patios);
        
        // filtro por Nome, Bairro ou Cnpj
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(f =>
                (f.Nome   ?? string.Empty).Contains(term) ||
                (f.Bairro ?? string.Empty).Contains(term) ||
                (f.Cnpj   ?? string.Empty).Contains(term));
        }
        
        // ordenação padrão: Nome ASC, depois Id
        query = query
            .OrderBy(f => f.Nome)
            .ThenBy(f => f.Id);
        
        // total antes de paginar
        var total = await query.LongCountAsync(ct);
        
        // página solicitada (Skip/Take)
        IReadOnlyList<Filial> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        // monta PageResult no formato padrão
        return new PageResult<Filial>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public async Task<Filial?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // busca por id
        return await context.Filiais
            .AsNoTracking()
            .Include(f => f.Patios)
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(Filial filial, CancellationToken ct = default)
    {
        // anexa e marca como modificada para persistir alterações vindas de fora do contexto
        context.Attach(filial);
        context.Entry(filial).State = EntityState.Modified;
        
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        // localiza a entidade, se não existir retorna false
        var entity = await context.Filiais
            .FirstOrDefaultAsync(f => f.Id == id, ct);
        if(entity == null) return false;
        
        // remove e salva
        context.Filiais.Remove(entity);
        var saved = await context.SaveChangesAsync(ct);
        return saved > 0;
        
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.API.Controllers
{
    [Route("api/[controller]")]
    [Tags("Patios")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly VisionHiveContext _context;

        public PatioController(VisionHiveContext context)
        {
            _context = context;
        }

        /// <summary>Lista todos os pátios com sua filial e motos</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatioResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatios(CancellationToken ct)
        {
            var patios = await _context.Patios
                .Include(p => p.Filial)
                .Include(p => p.Motos)
                .AsSplitQuery()
                .AsNoTracking()
                .OrderBy(p => p.Nome)
                .Select(p => new PatioResponse
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    LimiteMotos = p.LimiteMotos,
                    FilialId = p.FilialId,
                    Filial = p.Filial.Nome,
                    Motos = p.Motos
                        .OrderBy(m => m.Prioridade)
                        .Select(m => new MotoResponse
                        {
                            Id = m.Id,
                            Placa = m.Placa,
                            Chassi = m.Chassi,
                            NumeroMotor = m.NumeroMotor,
                            Prioridade = m.Prioridade.ToString(),
                            Patio = p.Nome
                        })
                        .ToList()
                })
                .ToListAsync(ct);

            return Ok(patios);
        }

        /// <summary>Busca um pátio por id (com filial e motos)</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PatioResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PatioResponse>> GetPatio(Guid id, CancellationToken ct)
        {
            var patio = await _context.Patios
                .Include(p => p.Filial)
                .Include(p => p.Motos)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PatioResponse
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    LimiteMotos = p.LimiteMotos,
                    FilialId = p.FilialId,
                    Filial = p.Filial.Nome,
                    Motos = p.Motos
                        .OrderBy(m => m.Prioridade)
                        .Select(m => new MotoResponse
                        {
                            Id = m.Id,
                            Placa = m.Placa,
                            Chassi = m.Chassi,
                            NumeroMotor = m.NumeroMotor,
                            Prioridade = m.Prioridade.ToString(),
                            Patio = p.Nome
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (patio is null) return NotFound();
            return Ok(patio);
        }

        /// <summary>Cadastra um novo pátio</summary>
        [HttpPost]
        [ProducesResponseType(typeof(PatioResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PatioResponse>> PostPatio([FromBody] PatioRequest request, CancellationToken ct)
        {
            // 1) valida filial (sem subconsulta aninhada)
            var filial = await _context.Filiais
                .AsNoTracking()
                .Where(f => f.Id == request.FilialId)
                .Select(f => new { f.Id, f.Nome })
                .FirstOrDefaultAsync(ct);

            if (filial is null)
                return NotFound("Filial não encontrada.");

            // 2) cria e persiste
            var patio = new VisionHive.Domain.Entities.Patio(request.Nome, request.LimiteMotos, request.FilialId);
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync(ct);

            // 3) carrega coleções via ChangeTracker e monta DTO em memória (evita WHERE FALSE no Oracle)
            await _context.Entry(patio).Collection(p => p.Motos).LoadAsync(ct);

            var response = new PatioResponse
            {
                Id = patio.Id,
                Nome = patio.Nome,
                LimiteMotos = patio.LimiteMotos,
                FilialId = filial.Id,
                Filial  = filial.Nome,
                Motos = patio.Motos
                    .OrderBy(m => m.Prioridade)
                    .Select(m => new MotoResponse
                    {
                        Id = m.Id,
                        Placa = m.Placa,
                        Chassi = m.Chassi,
                        NumeroMotor = m.NumeroMotor,
                        Prioridade = m.Prioridade.ToString(),
                        Patio = patio.Nome
                    })
                    .ToList()
            };

            return CreatedAtAction(nameof(GetPatio), new { id = response.Id }, response);
        }

        /// <summary>Atualiza os dados de um pátio</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutPatio(Guid id, [FromBody] PatioRequest request, CancellationToken ct)
        {
            var patio = await _context.Patios.FindAsync(new object?[] { id }, ct);
            if (patio is null) return NotFound();

            patio.AtualizarDados(request.Nome, request.LimiteMotos);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }

        /// <summary>Remove um pátio</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePatio(Guid id, CancellationToken ct)
        {
            var patio = await _context.Patios.FindAsync(new object?[] { id }, ct);
            if (patio is null) return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}

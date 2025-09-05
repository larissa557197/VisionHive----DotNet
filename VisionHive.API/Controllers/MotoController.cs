using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.API.Controllers
{
    [Route("api/[controller]")]
    [Tags("Motos")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly VisionHiveContext _context;

        public MotoController(VisionHiveContext context)
        {
            _context = context;
        }

        /// <summary>Retorna uma lista de motos</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotoResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public async Task<ActionResult<IEnumerable<MotoResponse>>> GetMotos(CancellationToken ct)
        {
            // Projeção simples, sem subconsultas
            var motos = await _context.Motos
                .Include(m => m.Patio)
                .AsSplitQuery()
                .AsNoTracking()
                .Select(m => new MotoResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Chassi = m.Chassi,
                    NumeroMotor = m.NumeroMotor,
                    Prioridade = m.Prioridade.ToString(),
                    Patio = m.Patio.Nome
                })
                .ToListAsync(ct);

            return Ok(motos);
        }

        /// <summary>Retorna uma moto pelo ID</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MotoResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MotoResponse>> GetMoto(Guid id, CancellationToken ct)
        {
            var moto = await _context.Motos
                .Include(m => m.Patio)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MotoResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Chassi = m.Chassi,
                    NumeroMotor = m.NumeroMotor,
                    Prioridade = m.Prioridade.ToString(),
                    Patio = m.Patio.Nome
                })
                .FirstOrDefaultAsync(ct);

            if (moto is null) return NotFound();
            return Ok(moto);
        }

        /// <summary>Cadastra uma nova moto</summary>
        [HttpPost]
        [ProducesResponseType(typeof(MotoResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MotoResponse>> PostMoto([FromBody] MotoRequest request, CancellationToken ct)
        {
            // 1) valida pátio (sem subconsulta aninhada em projeção)
            var patio = await _context.Patios
                .AsNoTracking()
                .Where(p => p.Id == request.PatioId)
                .Select(p => new { p.Id, p.Nome })
                .FirstOrDefaultAsync(ct);

            if (patio is null)
                return NotFound("Pátio informado não existe.");

            // 2) cria e persiste
            var moto = new VisionHive.Domain.Entities.Moto(
                request.Placa, request.Chassi, request.NumeroMotor, request.Prioridade, request.PatioId);

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync(ct);

            // 3) monta o DTO em memória (evita WHERE FALSE no Oracle)
            var response = new MotoResponse
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Chassi = moto.Chassi,
                NumeroMotor = moto.NumeroMotor,
                Prioridade = moto.Prioridade.ToString(),
                Patio = patio.Nome
            };

            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, response);
        }

        /// <summary>Atualiza uma moto existente</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutMoto(Guid id, [FromBody] MotoRequest request, CancellationToken ct)
        {
            var moto = await _context.Motos.FindAsync(new object?[] { id }, ct);
            if (moto is null) return NotFound();

            // valida pátio alvo
            var patioExiste = await _context.Patios
                .AsNoTracking()
                .AnyAsync(p => p.Id == request.PatioId, ct);

            if (!patioExiste)
                return NotFound("Pátio informado não existe.");

            moto.AtualizarDados(request.Placa, request.Chassi, request.NumeroMotor, request.Prioridade, request.PatioId);
            await _context.SaveChangesAsync(ct); // tracked; não precisa Update

            return NoContent();
        }

        /// <summary>Remove uma moto existente</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteMoto(Guid id, CancellationToken ct)
        {
            var moto = await _context.Motos.FindAsync(new object?[] { id }, ct);
            if (moto is null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}

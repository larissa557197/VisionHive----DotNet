using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.API.Controllers
{
    [Route("api/[controller]")]
    [Tags("Filiais")]
    [ApiController]
    public class FilialController : ControllerBase
    {
        private readonly VisionHiveContext _context;
        public FilialController(VisionHiveContext context) => _context = context;

        /// <summary>Lista todas as filiais com seus pátios (e motos)</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FilialResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FilialResponse>>> GetFiliais(CancellationToken ct)
        {
            var filiais = await _context.Filiais
                .Include(f => f.Patios)
                    .ThenInclude(p => p.Motos)
                .AsNoTracking()
                .Select(f => new FilialResponse
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Bairro = f.Bairro,
                    Cnpj = f.Cnpj,
                    Patios = f.Patios.Select(p => new PatioResponse
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        LimiteMotos = p.LimiteMotos,
                        Motos = p.Motos.Select(m => new MotoResponse
                        {
                            Id = m.Id,
                            Placa = m.Placa,
                            Chassi = m.Chassi,
                            NumeroMotor = m.NumeroMotor,
                            Prioridade = m.Prioridade.ToString(), // enum -> string
                            Patio = p.Nome                         // nome do pátio pai
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync(ct);

            return Ok(filiais);
        }

        /// <summary>Busca uma filial por id (com pátios e motos)</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FilialResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<FilialResponse>> GetFilial(Guid id, CancellationToken ct)
        {
            var filial = await _context.Filiais
                .Include(f => f.Patios)
                    .ThenInclude(p => p.Motos)
                .AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => new FilialResponse
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Bairro = f.Bairro,
                    Cnpj = f.Cnpj,
                    Patios = f.Patios.Select(p => new PatioResponse
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        LimiteMotos = p.LimiteMotos,
                        Motos = p.Motos.Select(m => new MotoResponse
                        {
                            Id = m.Id,
                            Placa = m.Placa,
                            Chassi = m.Chassi,
                            NumeroMotor = m.NumeroMotor,
                            Prioridade = m.Prioridade.ToString(),
                            Patio = p.Nome
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (filial is null) return NotFound();
            return Ok(filial);
        }

        /// <summary>Cadastra uma nova filial</summary>
        [HttpPost]
        [ProducesResponseType(typeof(FilialResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<FilialResponse>> PostFilial([FromBody] FilialRequest request, CancellationToken ct)
        {
            var filial = new VisionHive.Domain.Entities.Filial(request.Nome, request.Bairro, request.Cnpj);

            _context.Filiais.Add(filial);
            await _context.SaveChangesAsync(ct);

            var response = new FilialResponse
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Bairro = filial.Bairro,
                Cnpj = filial.Cnpj,
                Patios = new List<PatioResponse>()
            };

            return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, response);
        }

        /// <summary>Atualiza dados de uma filial</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutFilial(Guid id, [FromBody] FilialRequest request, CancellationToken ct)
        {
            var filial = await _context.Filiais.FindAsync(new object?[] { id }, ct);
            if (filial is null) return NotFound();

            filial.AtualizarDados(request.Nome, request.Bairro, request.Cnpj);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }

        /// <summary>Remove uma filial</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteFilial(Guid id, CancellationToken ct)
        {
            var filial = await _context.Filiais.FindAsync(new object?[] { id }, ct);
            if (filial is null) return NotFound();

            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}

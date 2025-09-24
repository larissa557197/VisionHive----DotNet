using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Application.UseCases;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;

/// <summary>Endpoints REST para a entidade Pátio.</summary>
namespace VisionHive.API.Controllers
{
    [Route("api/v1/patios")]
    //[Tags("Patios")]
    [ApiController]
    public class PatioController(IPatioUseCase patioUseCase) : ControllerBase
    {
        /// <summary>Cria um novo pátio.</summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PatioRequest request)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await patioUseCase.CreateAsync(request);
                // 201 com body
                return StatusCode((int)HttpStatusCode.Created, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Lista pátios com paginação e filtro.</summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PageResult<PatioResponse>>> GetPaginate([FromQuery] PatioPaginatedRequest query)
        {
            var page = await patioUseCase.GetPagination(query);
            var mapped = new PageResult<PatioResponse>
            {
                Items = page.Items.Select(p => new PatioResponse
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    LimiteMotos = p.LimiteMotos,
                    FilialId = p.FilialId,
                    Filial = p.Filial?.Nome ?? string.Empty,
                    Motos = p.Motos.Select(m => new MotoResponse
                    {
                        Placa = m.Placa,
                        Prioridade = m.Prioridade.ToString()
                    }).ToList()
                }).ToList(),
                Page = page.Page,
                PageSize = page.PageSize,
                Total = page.Total
            };
            return Ok(mapped);
        }

        /// <summary>Obtém um pátio por ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PatioResponse>> GetById(Guid id)
        {
            var entity = await patioUseCase.GetByIdAsync(id);
            if (entity is null) return NotFound("Pátio não encontrado");

            var dto = new PatioResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                LimiteMotos = entity.LimiteMotos,
                FilialId = entity.FilialId,
                Filial = entity.Filial?.Nome ?? string.Empty,
                Motos = entity.Motos.Select(m => new MotoResponse
                {
                    Placa = m.Placa,
                    Prioridade = m.Prioridade.ToString()
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>Atualiza um pátio existente.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] PatioRequest request)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);
            try
            {
                var ok = await patioUseCase.UpdateAsync(id, request);
                return ok ? NoContent() : NotFound("Pátio não encontrado");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove um pátio pelo ID.</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await patioUseCase.DeleteAsync(id);
            return ok ? NoContent() : NotFound("Pátio não encontrado");
        }
    }
}

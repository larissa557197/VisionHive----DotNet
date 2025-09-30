using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Swashbuckle.AspNetCore.Filters;
using VisionHive.API.SwaggerExamples;
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
        [SwaggerRequestExample(typeof(PatioRequest), typeof(PatioRequestExample))]           //  exemplo de request
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(PatioResponseExample))] //  exemplo de response
        public async Task<IActionResult> Post([FromBody] PatioRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await patioUseCase.CreateAsync(request);

                var links = new
                {
                    self   = new { href = Url.ActionLink(nameof(GetById), values: new { id = created.Id }), method = "GET" },
                    update = new { href = Url.ActionLink(nameof(Put),     values: new { id = created.Id }), method = "PUT" },
                    delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id = created.Id }), method = "DELETE" }
                };

                // 201 + Location + HATEOAS
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created, _links = links });
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
        public async Task<IActionResult> GetPaginate([FromQuery] PatioPaginatedRequest query)
        {
            var page = await patioUseCase.GetPagination(query);

            // itens com link próprio
            var items = page.Items.Select(p => new
            {
                data = new PatioResponse
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
                },
                _links = new
                {
                    self = new { href = Url.ActionLink(nameof(GetById), values: new { id = p.Id }), method = "GET" }
                }
            });

            // links de navegação paginada
            var self = Url.ActionLink(nameof(GetPaginate), values: query);
            var next = page.HasNext ? Url.ActionLink(nameof(GetPaginate), values: new
            {
                PageNumber = page.Page + 1,
                PageSize = page.PageSize,
                query.Search,
                query.SortBy,
                query.SortDir
            }) : null;

            var prev = page.HasPrevious ? Url.ActionLink(nameof(GetPaginate), values: new
            {
                PageNumber = page.Page - 1,
                PageSize = page.PageSize,
                query.Search,
                query.SortBy,
                query.SortDir
            }) : null;

            return Ok(new
            {
                items,
                page = page.Page,
                pageSize = page.PageSize,
                totalItems = page.Total,
                totalPages = page.TotalPages,
                _links = new { self, next, prev }
            });
        }

        /// <summary>Obtém um pátio por ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
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

            var links = new
            {
                self   = new { href = Url.ActionLink(nameof(GetById), values: new { id }), method = "GET" },
                update = new { href = Url.ActionLink(nameof(Put),     values: new { id }), method = "PUT" },
                delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id }), method = "DELETE" }
            };

            return Ok(new { data = dto, _links = links });
        }

        /// <summary>Atualiza um pátio existente.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerRequestExample(typeof(PatioRequest), typeof(PatioRequestExample))] //  opcional: exemplo no PUT também
        public async Task<IActionResult> Put(Guid id, [FromBody] PatioRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
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

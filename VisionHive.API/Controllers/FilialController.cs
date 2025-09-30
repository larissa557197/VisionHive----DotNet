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

namespace VisionHive.API.Controllers
{
    [Route("api/v1/filiais")]
    //[Tags("Filiais")]
    [ApiController]
    public class FilialController(IFilialUseCase filialUseCase) : ControllerBase
    {
        /// <summary>Cria uma nova filial.</summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerRequestExample(typeof(FilialRequest), typeof(FilialRequestExample))] // exemplo de request
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(FilialResponseExample))] // exemplo de response
        public async Task<IActionResult> Post([FromBody] FilialRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await filialUseCase.CreateAsync(request);

                // 201 + Location com HATEOAS do recurso criado
                var links = new
                {
                    self   = new { href = Url.ActionLink(nameof(GetById), values: new { id = created.Id }), method = "GET" },
                    update = new { href = Url.ActionLink(nameof(Put),     values: new { id = created.Id }), method = "PUT" },
                    delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id = created.Id }), method = "DELETE" }
                };

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created, _links = links });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Lista filiais com paginação e filtro.</summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginate([FromQuery] FilialPaginatedRequest query)
        {
            var page = await filialUseCase.GetPagination(query);

            // itens com link próprio
            var items = page.Items.Select(f => new
            {
                data = new FilialResponse
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Bairro = f.Bairro,
                    Cnpj = f.Cnpj,
                    Patios = f.Patios.Select(p => p.Nome).ToList()
                },
                _links = new
                {
                    self = new { href = Url.ActionLink(nameof(GetById), values: new { id = f.Id }), method = "GET" }
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

        /// <summary>Obtém uma filial por ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await filialUseCase.GetByIdAsync(id);
            if (entity is null) return NotFound("Filial não encontrada");

            var dto = new FilialResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Bairro = entity.Bairro,
                Cnpj = entity.Cnpj,
                Patios = entity.Patios.Select(p => p.Nome).ToList()
            };

            var links = new
            {
                self   = new { href = Url.ActionLink(nameof(GetById), values: new { id }), method = "GET" },
                update = new { href = Url.ActionLink(nameof(Put),     values: new { id }), method = "PUT" },
                delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id }), method = "DELETE" }
            };

            return Ok(new { data = dto, _links = links });
        }

        /// <summary>Atualiza uma filial existente.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerRequestExample(typeof(FilialRequest), typeof(FilialRequestExample))] // opcional: exemplo no PUT também
        public async Task<IActionResult> Put(Guid id, [FromBody] FilialRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var ok = await filialUseCase.UpdateAsync(id, request);
                return ok ? NoContent() : NotFound("Filial não encontrada.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Remove uma filial pelo ID.</summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await filialUseCase.DeleteAsync(id);
            return ok ? NoContent() : NotFound("Filial não encontrada.");
        }
    }
}

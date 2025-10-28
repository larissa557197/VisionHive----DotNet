using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Swashbuckle.AspNetCore.Filters;
using VisionHive.API.SwaggerExamples;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Domain.Enums;
using VisionHive.Application.UseCases;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.API.Controllers
{
    /// <summary>
    /// Endpoints REST para a entidade Moto
    /// </summary>
    [Route("api/v1/motos")]
    //[Tags("Motos")]
    [ApiController]
    public class MotoController(IMotoUseCase motoUseCase) : ControllerBase
    {
        /// <summary>
        /// Cria uma nova moto
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerRequestExample(typeof(MotoRequest), typeof(MotoRequestExample))] //  exemplo de request
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(MotoResponseExample))] //  exemplo de response
        public async Task<IActionResult> Post([FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await motoUseCase.CreateAsync(request);

                var links = new
                {
                    self   = new { href = Url.ActionLink(nameof(GetById), values: new { id = created.Id }), method = "GET" },
                    update = new { href = Url.ActionLink(nameof(Put),     values: new { id = created.Id }), method = "PUT" },
                    delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id = created.Id }), method = "DELETE" }
                };

                // 201 + Location + HATEOAS do recurso criado
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created, _links = links });
            }
            catch (ArgumentException ex)
            {
                // regra de negócio (ex.: precisa Placa/Chassi/NumeroMotor)
                return BadRequest(ex.Message);
            }
        }

        ///<summary> Lista motos com paginação e filtro </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginate([FromQuery] MotoPaginatedRequest query)
        {
            var page = await motoUseCase.GetPagination(query);

            // itens com link próprio
            var items = page.Items.Select(m => new
            {
                data = new MotoResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Chassi = m.Chassi,
                    NumeroMotor = m.NumeroMotor,
                    Prioridade = m.Prioridade.ToString(),
                    PatioId = m.PatioId,
                    Patio = m.Patio?.Nome
                },
                _links = new
                {
                    self = new { href = Url.ActionLink(nameof(GetById), values: new { id = m.Id }), method = "GET" }
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

        /// <summary>
        /// Obtém uma moto por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await motoUseCase.GetByIdAsync(id);
            if (entity is null) return NotFound("Moto não encontrada");

            var dto = new MotoResponse
            {
                Id = entity.Id,
                Placa = entity.Placa,
                Chassi = entity.Chassi,
                NumeroMotor = entity.NumeroMotor,
                Prioridade = entity.Prioridade.ToString(),
                PatioId = entity.PatioId,
                Patio = entity.Patio?.Nome
            };

            var links = new
            {
                self   = new { href = Url.ActionLink(nameof(GetById), values: new { id }), method = "GET" },
                update = new { href = Url.ActionLink(nameof(Put),     values: new { id }), method = "PUT" },
                delete = new { href = Url.ActionLink(nameof(Delete),  values: new { id }), method = "DELETE" }
            };

            return Ok(new { data = dto, _links = links });
        }

        /// <summary>
        /// Atualiza uma moto existente
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [SwaggerRequestExample(typeof(MotoRequest), typeof(MotoRequestExample))] //  opcional: request example no PUT
        public async Task<IActionResult> Put(Guid id, [FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var ok = await motoUseCase.UpdateAsync(id, request);
                return ok ? NoContent() : NotFound("Moto não encontrada");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma moto pelo ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await motoUseCase.DeleteAsync(id);
            return ok ? NoContent() : NotFound("Moto não encontrada");
        }
    }
}

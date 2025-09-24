using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        public async Task<IActionResult> Post([FromBody] FilialRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await filialUseCase.CreateAsync(request);

                //201 com body
                return StatusCode((int)HttpStatusCode.Created, created);
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
        public async Task<ActionResult<PageResult<FilialResponse>>> GetPaginate([FromQuery] FilialPaginatedRequest query)
        {
            var page = await filialUseCase.GetPagination(query);
            var mapped = new PageResult<FilialResponse>
            {
                Items = page.Items.Select(f => new FilialResponse
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Bairro = f.Bairro,
                    Cnpj = f.Cnpj,
                    Patios = f.Patios.Select(p => p.Nome).ToList()
                }).ToList(),
                Page = page.Page,
                PageSize = page.PageSize,
                Total = page.Total
            };
            return Ok(mapped);
        }

        /// <summary>Obtém uma filial por ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<FilialResponse>> GetById(Guid id)
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
            return Ok(dto);
        }
        
        /// <summary>Atualiza uma filial existente.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Application.UseCases;
using VisionHive.Infrastructure.Contexts;

/// <summary>Endpoints REST para a entidade Pátio.</summary>
namespace VisionHive.API.Controllers
{
    [Route("api/")]
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
        public async Task<IActionResult> GetPaginate([FromQuery] PatioPaginatedRequest query)
        {
            var result = await patioUseCase.GetPagination(query);
            return Ok(result);
        }

        /// <summary>Obtém um pátio por ID.</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await patioUseCase.GetByIdAsync(id);
            return entity is null ? NotFound("Pátio não encontrado") : Ok(entity);
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

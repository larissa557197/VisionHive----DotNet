using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        /// cria uma nova moto
        /// </summary>
        /// <param name="motoRepository"></param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await motoUseCase.CreateAsync(request);
                return StatusCode((int)HttpStatusCode.Created, created);
            }
            catch (ArgumentException ex)
            {
                // regras de negócio ex: precisa Placa/Chassi/NumeroMotor
                return BadRequest(ex.Message);

            }
        }

        ///<summary> Lista motos com paginação e filtro </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<MotoResponse>> GetPaginate([FromQuery] MotoPaginatedRequest query)
        {
            var page = await motoUseCase.GetPagination(query);
            var mapped = new PageResult<MotoResponse>
            {
                Items = page.Items.Select(m => new MotoResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Chassi = m.Chassi,
                    NumeroMotor = m.NumeroMotor,
                    Prioridade = m.Prioridade.ToString(),
                    PatioId = m.PatioId,
                    Patio = m.Patio?.Nome
                }).ToList(),
                Page = page.Page,
                PageSize = page.PageSize,
                Total = page.Total
            };
            
            return Ok(mapped);
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
            return Ok(dto);
        }


        /// <summary>
        /// Atualiza uma moto existente
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

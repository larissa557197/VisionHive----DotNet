using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Infrastructure.Repositories.Mongo;

namespace VisionHive.API.Controllers.v2
{
    [Authorize]
    [Route("api/v{apiVersion:apiVersion}/motos")]
    [ApiController]
    [Asp.Versioning.ApiVersion(2.0)]
    public class MotoControllerV2 : ControllerBase
    {
        private readonly MotoMongoRepository _repository;

        public MotoControllerV2(MotoMongoRepository repository)
        {
            _repository = repository;
        }
        
        /// <summary>
        /// POST - Cria uma nova Moto (Mongo)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Moto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var moto = new Moto
            {
                Placa = request.Placa,
                Chassi = request.Chassi,
                NumeroMotor = request.NumeroMotor,
                Prioridade = request.Prioridade,
                FilialId = request.FilialId,
                PatioId = request.PatioId
            };

            await _repository.CreateAsync(moto);

            return Ok(new
            {
                Mensagem = "Motocicleta cadastrada com sucesso (MongoDB).",
                Data = moto
            });
        }
        
        /// <summary>
        /// GET - Lista todas as Motos (Mongo)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Moto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var motos = await _repository.GetAllAsync();
            return Ok(motos);
        }
        
        
        /// <summary>
        /// GET - Busca uma Moto pelo ID (Mongo)
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Moto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var moto = await _repository.GetByIdAsync(id);

            if (moto == null)
                return NotFound(new { Mensagem = $"Motocicleta com ID {id} não encontrada." });

            return Ok(moto);
        }
        
        
        /// <summary>
        /// PUT - Aatualiza Moto existente (Mongo)
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Motocicleta não encontrada." });

            existente.Placa = request.Placa;
            existente.Chassi = request.Chassi;
            existente.NumeroMotor = request.NumeroMotor;
            existente.Prioridade = request.Prioridade;
            existente.FilialId = request.FilialId;
            existente.PatioId = request.PatioId;

            var atualizado = await _repository.UpdateAsync(existente);
            if (!atualizado)
                return BadRequest(new { Mensagem = "Falha ao atualizar motocicleta." });

            return Ok(new
            {
                Mensagem = "Motocicleta atualizada com sucesso (MongoDB).",
                Data = existente
            });
        }
        
        
        /// <summary>
        /// DELETE - Remove uma Moto pelo ID (Mongo)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletado = await _repository.DeleteAsync(id);
            if (!deletado)
                return NotFound(new { Mensagem = "Motocicleta não encontrada." });

            return Ok(new { Mensagem = "Motocicleta excluída com sucesso (MongoDB)." });
        }
        
    }
        
}
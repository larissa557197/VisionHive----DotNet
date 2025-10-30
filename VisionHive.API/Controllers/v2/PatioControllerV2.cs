using Microsoft.AspNetCore.Mvc;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Infrastructure.Repositories.Mongo;

namespace VisionHive.API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/patios")]
    [Asp.Versioning.ApiVersion(2.0)]
    [ApiController]
    public class PatioControllerV2 : ControllerBase
    {
        private readonly PatioMongoRepository _repository;

        public PatioControllerV2(PatioMongoRepository repository)
        {
            _repository = repository;
        }
        
        /// <summary>
        /// POST - Cria um novo Pátio (Mongo)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Patio), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] PatioRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var patio = new Patio
            {
                Nome = request.Nome,
                LimiteMotos = request.LimiteMotos,
                FilialId = request.FilialId
            };

            await _repository.CreateAsync(patio);

            return Ok(new
            {
                Mensagem = "Pátio criado com sucesso (MongoDB).",
                Data = patio
            });
        }
        
        ////// <summary>
        /// GET - Lista todos os Pátios (Mongo)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Patio>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var patios = await _repository.GetAllAsync();
            return Ok(patios);
        }
        
        /// <summary>
        /// GET - Busca um pátio pelo ID (Mongo)
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Patio), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var patio = await _repository.GetByIdAsync(id);

            if (patio == null)
                return NotFound(new { Mensagem = $"Pátio com ID {id} não encontrado." });

            return Ok(patio);
        }
        
        /// <summary>
        /// PUT - Atualiza um pátio existente (Mongo)
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] PatioRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Pátio não encontrado." });

            existente.Nome = request.Nome;
            existente.LimiteMotos = request.LimiteMotos;
            existente.FilialId = request.FilialId;

            var atualizado = await _repository.UpdateAsync(existente);
            if (!atualizado)
                return BadRequest(new { Mensagem = "Falha ao atualizar o pátio." });

            return Ok(new
            {
                Mensagem = "Pátio atualizado com sucesso (MongoDB).",
                Data = existente
            });
        }
        
        /// <summary>
        /// DELETE - Remove um pátio pelo ID (Mongo)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletado = await _repository.DeleteAsync(id);
            if (!deletado)
                return NotFound(new { Mensagem = "Pátio não encontrado." });

            return Ok(new { Mensagem = "Pátio excluído com sucesso (MongoDB)." });
        }
    }
}
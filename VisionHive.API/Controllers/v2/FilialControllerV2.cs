using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Infrastructure.Repositories.Mongo;

namespace VisionHive.API.Controllers.v2
{
    [Authorize]
    [ApiController]
    [Asp.Versioning.ApiVersion(2.0)]
    [Route("api/v{apiVersion:apiVersion}/filiais")]
 
    public class FilialControllerV2 : ControllerBase
    {
        private readonly FilialMongoRepository _repository;

        public FilialControllerV2(FilialMongoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// POST - Cria uma nova Filial (Mongo)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Filial), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] FilialRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var filial = new Filial
            {
                Nome = request.Nome,
                Bairro = request.Bairro,
                Cnpj = request.Cnpj
            };

            await _repository.CreateAsync(filial);

            return Ok(new
            {
                Mensagem = "Filial inserida com sucesso (MongoDB).",
                Data = filial
            });
        }

        /// <summary>
        /// GET - Lista todas as  Filiais (Mongo)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Filial>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var filiais = await _repository.GetAllAsync();
            return Ok(filiais);
        }
        
        /// <summary>
        /// GET - Busca uma Filial pelo ID (Mongo)
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Filial), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var filial = await _repository.GetByIdAsync(id);

            if (filial == null)
                return NotFound(new { Mensagem = $"Filial com ID {id} não encontrada." });

            return Ok(filial);
        }
        
        /// <summary>
        /// PUT - Atualiza uma filial existente (MongoDB)
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] FilialRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Filial não encontrada." });

            existente.Nome = request.Nome;
            existente.Bairro = request.Bairro;
            existente.Cnpj = request.Cnpj;

            var atualizado = await _repository.UpdateAsync(existente);
            if (!atualizado)
                return BadRequest(new { Mensagem = "Falha ao atualizar filial." });

            return Ok(new
            {
                Mensagem = "Filial atualizada com sucesso (MongoDB).",
                Data = existente
            });
        }
        
        /// <summary>
        /// DELETE - Remove uma filial existente (MongoDB)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletado = await _repository.DeleteAsync(id);
            if (!deletado)
                return NotFound(new { Mensagem = "Filial não encontrada." });

            return Ok(new { Mensagem = "Filial excluída com sucesso (MongoDB)." });
        }
        
    }
}


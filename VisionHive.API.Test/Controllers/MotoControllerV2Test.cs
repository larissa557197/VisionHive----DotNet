using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionHive.API.Controllers.v2;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Enums;
using VisionHive.Infrastructure.Repositories.Mongo;
using Xunit;

namespace VisionHive.API.Test.Controllers;

public class MotoControllerV2Test
{
    // 🔹 Fake repository (sem precisar de Mongo real)
    private class FakeMotoMongoRepository : MotoMongoRepository
    {
        public FakeMotoMongoRepository() : base(null!) { }

        public Moto? UltimaMotoCriada { get; private set; }

        public override async Task<Moto> CreateAsync(Moto moto)
        {
            moto.Id = Guid.NewGuid();
            UltimaMotoCriada = moto;
            return await Task.FromResult(moto);
        }

        public override Task<List<Moto>> GetAllAsync()
            => Task.FromResult(new List<Moto>());
        
        public override Task<Moto?> GetByIdAsync(Guid id)
            => Task.FromResult<Moto?>(null);

        public override Task<bool> UpdateAsync(Moto moto)
            => Task.FromResult(true);

        public override Task<bool> DeleteAsync(Guid id)
            => Task.FromResult(true);
    }

    [Fact]
    public async Task Post_DeveRetornarOk_ComMensagemDeSucesso()
    {
        // Arrange
        var repo = new FakeMotoMongoRepository();
        var controller = new MotoControllerV2(repo)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new System.Security.Claims.ClaimsPrincipal()
                }
            }
        };

        var request = new MotoRequest
        {
            Placa = "ABC1D23",
            Chassi = "XYZ987654321",
            NumeroMotor = "MTR-123",
            Prioridade = Prioridade.Alta,
            FilialId = Guid.NewGuid(),
            PatioId = Guid.NewGuid()
        };

        // Act
        var result = await controller.Post(request);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(200);
        repo.UltimaMotoCriada.Should().NotBeNull();
        repo.UltimaMotoCriada!.Placa.Should().Be("ABC1D23");
    }
}

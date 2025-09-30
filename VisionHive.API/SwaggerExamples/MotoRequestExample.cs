using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Enums;

namespace VisionHive.API.SwaggerExamples;

public class MotoRequestExample : IExamplesProvider<MotoRequest>
{
    public MotoRequest GetExamples() => new MotoRequest
    {
        Placa = "ABC1D23",
        Chassi = "9BWZZZ377VT004251",
        NumeroMotor = "ENG12345",
        Prioridade = Prioridade.Media,
        PatioId = Guid.Parse("11111111-1111-1111-1111-111111111111")
    };
    
    
}
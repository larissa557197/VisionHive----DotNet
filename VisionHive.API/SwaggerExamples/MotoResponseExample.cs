using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Response;

namespace VisionHive.API.SwaggerExamples;

public class MotoResponseExample: IExamplesProvider<MotoResponse>
{
   public MotoResponse GetExamples() => new MotoResponse
   {
      Id = Guid.NewGuid(),
      Placa = "ABC1D23",
      Chassi = "9BWZZZ377VT004251",
      NumeroMotor = "ENG12345",
      Prioridade = "Média",
      PatioId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
      Patio = "Pátio Central"
   };
}
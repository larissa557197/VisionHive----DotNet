using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Response;

namespace VisionHive.API.SwaggerExamples;

public class FilialResponseExample : IExamplesProvider<FilialResponse>
{
    public FilialResponse GetExamples() => new FilialResponse
    {
        Id = Guid.NewGuid(),
        Nome = "Filial Zona Norte",
        Bairro = "Santana",
        Cnpj = "12.345.678/0001-99",
        Patios = new List<string> { "Pátio Central", "Pátio Secundário" }
    };
}
using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Request;

namespace VisionHive.API.SwaggerExamples;

public class FilialRequestExample : IExamplesProvider<FilialRequest>
{
    public FilialRequest GetExamples() => new FilialRequest
    {
        Nome = "Filial Zona Norte",
        Bairro = "Santana",
        Cnpj = "12.345.678/0001-99"
    };
}
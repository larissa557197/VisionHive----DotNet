using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Request;

namespace VisionHive.API.SwaggerExamples;

public class PatioRequestExample : IExamplesProvider<PatioRequest>
{
    public PatioRequest GetExamples() => new PatioRequest
    {
        Nome = "Pátio Central",
        LimiteMotos = 100,
        FilialId = Guid.Parse("22222222-2222-2222-2222-222222222222")
    };
}
using Swashbuckle.AspNetCore.Filters;
using VisionHive.Application.DTO.Response;

namespace VisionHive.API.SwaggerExamples;

public class PatioResponseExample :  IExamplesProvider<PatioResponse>
{
    public PatioResponse GetExamples() => new PatioResponse
    {
        Id = Guid.NewGuid(),
        Nome = "Pátio Central",
        LimiteMotos = 100,
        FilialId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Filial = "Filial Zona Norte",
        Motos = new List<MotoResponse>()
    };

}
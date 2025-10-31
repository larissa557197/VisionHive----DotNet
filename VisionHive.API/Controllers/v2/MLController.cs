using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionHive.Application.ML;

namespace VisionHive.API.Controllers.v2;

[Authorize]
[ApiController]
[Asp.Versioning.ApiVersion(2.0)]
[Route("api/v{apiVersion:apiVersion}/ml")]
public class MLController : ControllerBase
{
    private readonly MotoMaintenanceService _service = new();

    /// <summary>
    /// Faz a previsão de necessidade de manutenção com base em km e tempo de uso.
    /// </summary>
    [HttpPost("predict")]
    public IActionResult Predict([FromBody] MotoMaintenanceModel input)
    {
        var resultado = _service.Predict(input.KmRodados, input.TempoUsoMeses);

        string risco = resultado.Score switch
        {
            > 0.7f => "ALTO",
            > 0.3f => "MÉDIO",
            _ => "BAIXO"
        };

        return Ok(new
        {
            mensagem = "Predição realizada com sucesso!",
            entrada = input,
            resultado = new
            {
                necessitaManutencao = resultado.Predito,
                risco,
                resultado.Score
            }
        });
    }
}


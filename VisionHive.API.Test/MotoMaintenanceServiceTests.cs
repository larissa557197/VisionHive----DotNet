using VisionHive.Application.ML;
using Xunit;

namespace VisionHive.API.Test;

public class MotoMaintenanceServiceTests
{
    private readonly MotoMaintenanceService _service;

    public MotoMaintenanceServiceTests()
    {
        _service = new MotoMaintenanceService();
    }

    [Fact]
    public void Predict_DeveRetornar_PredicaoValida()
    {
        // Arrange
        float kmRodados = 10000f;
        float tempoUsoMeses = 12f;

        // Act
        var resultado = _service.Predict(kmRodados, tempoUsoMeses);

        // Assert
        Assert.NotNull(resultado);
        Assert.IsType<MotoMaintenancePrediction>(resultado);

        // O Score deve ser um número real (não NaN)
        Assert.False(float.IsNaN(resultado.Score));

        // O campo Predito deve ser booleano
        Assert.IsType<bool>(resultado.Predito);
    }
}

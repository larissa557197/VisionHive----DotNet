using Microsoft.ML.Data;

namespace VisionHive.Application.ML;

public class MotoMaintenanceModel
{
    [LoadColumn(0)]
    public float KmRodados { get; set; }

    [LoadColumn(1)]
    public float TempoUsoMeses { get; set; }


    [LoadColumn(2), ColumnName("Label")]
    public bool NecessitaManutencao { get; set; } // Indica ao ML.NET que essa é a coluna Label
}
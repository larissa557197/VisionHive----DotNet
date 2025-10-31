using Microsoft.ML.Data;

namespace VisionHive.Application.ML;

public class MotoMaintenancePrediction
{
    [ColumnName("PredictedLabel")]
    public bool Predito { get; set; }
    public float Score { get; set; }
    
   
}
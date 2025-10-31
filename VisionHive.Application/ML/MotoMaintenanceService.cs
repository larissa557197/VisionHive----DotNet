using Microsoft.ML;

namespace VisionHive.Application.ML;

public class MotoMaintenanceService
{
    private readonly MLContext _context;
    private readonly ITransformer _model;

    public MotoMaintenanceService()
    {
        _context = new MLContext();
        
        // dados de treino fictícios
        var dadosTreino = new List<MotoMaintenanceModel>
        {
            new() {KmRodados = 5000, TempoUsoMeses = 6, NecessitaManutencao = false},
            new() {KmRodados = 15000, TempoUsoMeses = 12, NecessitaManutencao = true},
            new() {KmRodados = 8000, TempoUsoMeses = 10, NecessitaManutencao = true},
            new() {KmRodados = 3000, TempoUsoMeses = 4, NecessitaManutencao = false}
        };
        
        var trainData = _context.Data.LoadFromEnumerable(dadosTreino);
        
        // define pipeline
        var pipeline = _context.Transforms.Concatenate("Features", "KmRodados", "TempoUsoMeses")
            .Append(_context.BinaryClassification.Trainers.SdcaLogisticRegression());

        
        // treina modelo
        _model = pipeline.Fit(trainData);
    }
    
    public MotoMaintenancePrediction Predict(float kmRodados, float tempoUsoMeses)
    {
        var predictionEngine = _context.Model.CreatePredictionEngine<MotoMaintenanceModel, MotoMaintenancePrediction>(_model);

        var input = new MotoMaintenanceModel
        {
            KmRodados = kmRodados,
            TempoUsoMeses = tempoUsoMeses
        };

        return predictionEngine.Predict(input);
    }
}
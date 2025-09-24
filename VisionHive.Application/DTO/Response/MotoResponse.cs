namespace VisionHive.Application.DTO.Response
{
    public class MotoResponse
    {
        public Guid Id { get; init; }
        public string? Placa { get; init; }
        public string? Chassi { get; init; }
        public string? NumeroMotor { get; init; }
        public string Prioridade { get; init; }
        public Guid PatioId { get; init; }
        public string? Patio { get; init; }
    }
}

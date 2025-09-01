namespace VisionHive.Application.DTO.Response
{
    public class MotoResponse
    {
        public Guid Id { get; set; }
        public string? Placa { get; set; }
        public string? Chassi { get; set; }
        public string? NumeroMotor { get; set; }
        public string Prioridade { get; set; }
        public string Patio { get; set; }
    }
}

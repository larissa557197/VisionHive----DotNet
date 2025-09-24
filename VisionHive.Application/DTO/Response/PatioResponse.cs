namespace VisionHive.Application.DTO.Response
{
    public class PatioResponse
    {
        public Guid Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public int LimiteMotos { get; init; }
        
        // relacionamento com Filial
        public Guid FilialId { get; init; }
        // nome da filial
        public string Filial { get; init; } = string.Empty;
        public IReadOnlyList<MotoResponse> Motos { get; init; } = Array.Empty<MotoResponse>();
        
        
    }
}

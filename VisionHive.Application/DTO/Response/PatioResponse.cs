namespace VisionHive.Application.DTO.Response
{
    public class PatioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int LimiteMotos { get; set; }
        
        // relacionamento com Filial
        public Guid FilialId { get; set; }
        // nome da filial
        public string Filial { get; set; } = default!;
        public IReadOnlyList<string> Motos { get; init; } = Array.Empty<string>();
    }
}

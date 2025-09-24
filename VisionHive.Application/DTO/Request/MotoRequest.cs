using System.ComponentModel.DataAnnotations;
using VisionHive.Domain.Enums;
namespace VisionHive.Application.DTO.Request
{
    public class MotoRequest
    {
        // placa da moto
        public string? Placa { get; set; }
        
        // chassi da moto
        public string? Chassi { get; set; }
        
        // número do motor da moto
        public string? NumeroMotor { get; set; }
        
        
        // prioridade operacional para tratamento da moto
        [Required]
        public Prioridade Prioridade { get; set; }
        
        // identificador do pátio onde a moto está vinculada
        [Required]
        public Guid PatioId { get; set; }
    }
}

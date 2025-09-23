using System.ComponentModel.DataAnnotations;

namespace VisionHive.Application.DTO.Request
{
    /// <summary>
    /// Dados para criar ou atualizar um <c>Patio</c>
    /// Este DTO é usado pelos endpoints POST e PUT
    /// </summary>
    public class PatioRequest
    {
        /// <summary>
        /// Nome do pátio
        /// <example>Pátio Central</example>
        /// </summary>
        [Required(ErrorMessage = "O nome do pátio é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// Limite máximo de motos permitido no pátio
        /// A entidade de dominio também valida que seja maior que zero
        /// <example>100</example>
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O limite de motos deve ser maior que zero")]
        public int LimiteMotos { get; set; }
        
        /// <summary>
        /// Identificador da filial associada ao pátio
        /// </summary>
        [Required(ErrorMessage = "O Id da filial é obrigatório")]
        public Guid FilialId { get; set; }
    }
}

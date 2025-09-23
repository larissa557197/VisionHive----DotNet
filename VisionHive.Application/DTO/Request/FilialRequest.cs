using System.ComponentModel.DataAnnotations;

namespace VisionHive.Application.DTO.Request
{
    /// <summary>
    /// Dados para criar ou atualizar uma <c>Filial</c>.
    /// Este DTO é usado nos endpoints POST e PUT.
    /// </summary>
    public class FilialRequest
    {
        /// <summary>Nome da filial.</summary>
        /// <example>Filial São Paulo Centro</example>
        [Required(ErrorMessage = "O nome da filial é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>Bairro onde a filial está localizada.</summary>
        /// <example>Bela Vista</example>
        [Required(ErrorMessage = "O bairro é obrigatório")] 
        public string Bairro { get; set; }
        
        /// <summary>CNPJ da filial.</summary>
        /// <example>12.345.678/0001-99</example>
        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [RegularExpression(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "Formato de CNPJ inválido")]
        public string Cnpj { get; set; } =  string.Empty;
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VisionHive.Domain.Enums;

namespace VisionHive.Domain.Entities
{
    public class Moto
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string? Placa { get; set; }

        public string? Chassi { get; set; }

        public string? NumeroMotor { get; set; }

        public Prioridade Prioridade { get; set; }

        public Guid FilialId { get; set; }
             
        public Guid PatioId { get; set; }
        public Patio Patio { get; set; }

        public Moto(){}

        public Moto(string? placa, string? chassi, string? numeroMotor, Prioridade prioridade, Guid patioId)
        {
            Validar(placa, chassi, numeroMotor);

            Id = Guid.NewGuid();
            Placa = placa;
            Chassi = chassi;
            NumeroMotor = numeroMotor;
            Prioridade = prioridade;
            PatioId = patioId;

        }

        public void AtualizarDados(string? placa, string? chassi, string? numeroMotor, Prioridade prioridade, Guid patioId)
        {
            Validar(placa, chassi, numeroMotor);

            Placa = placa;
            Chassi = chassi;
            NumeroMotor = numeroMotor;
            Prioridade = prioridade;
            PatioId = patioId;

        }

        private void Validar(string? placa, string? chassi, string? numeroMotor)
        {
            if(string.IsNullOrWhiteSpace(placa) && string.IsNullOrWhiteSpace(chassi) && string.IsNullOrWhiteSpace(numeroMotor))
            {
                throw new Exception("É necessário informar pelo menos a placa, o chassi ou o número do motor.");
            }
        }

    }
}

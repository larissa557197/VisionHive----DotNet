

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VisionHive.Domain.Entities
{
    public class Filial
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id {  get; set; }
        public string Nome { get; set; }
        public string Bairro { get; set; }
        public string Cnpj { get; set; }

        public ICollection<Patio> Patios { get; set; } = new List<Patio>();

        
        public Filial(){}
        
        public Filial(string nome, string bairro, string cnpj)
        {
            Validar(nome, bairro, cnpj);
            Id = Guid.NewGuid();
            Nome = nome;
            Bairro = bairro;
            Cnpj = cnpj;
            Patios = new List<Patio>();
        }

        public void AtualizarDados(string nome, string bairro, string cnpj)
        {
            Validar(nome, bairro, cnpj);
            Nome = nome;
            Bairro = bairro;
            Cnpj = cnpj;
        }

        private void Validar(string nome, string bairro, string cnpj)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new Exception("O nome da filial não pode ser vazio.");
            }
            if (string.IsNullOrWhiteSpace(bairro))
            {
                throw new Exception("O bairro da filial não pode ser vazio.");
            }
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                throw new Exception("O CNPJ da filial não pode ser vazio.");
            }
        }
    }
}

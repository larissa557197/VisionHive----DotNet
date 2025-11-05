using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using VisionHive.Domain.Entities;

namespace VisionHive.Infrastructure.Repositories.Mongo;

public class PatioMongoRepository
{
     private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Patio> _collection;

    public PatioMongoRepository(IMongoDatabase database)
    {
        _database = database;

        // ✅ Garante compatibilidade com UUID binário padrão do Mongo
        try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch
        {
            // ignora se já tiver sido registrado (versões antigas não têm verificação)
        }

        _collection = _database.GetCollection<Patio>("Patios");
    }

    // CREATE
    public async Task<Patio> CreateAsync(Patio patio)
    {
        await _collection.InsertOneAsync(patio);
        return patio;
    }

    // READ - Todos
    public async Task<List<Patio>> GetAllAsync()
    {
        var patios = await _collection.Find(_ => true).ToListAsync();

        // Inclui o nome da Filial e as Motos vinculadas
        var filialCollection = _database.GetCollection<Filial>("Filiais");
        var motoCollection = _database.GetCollection<Moto>("Motos");

        foreach (var patio in patios)
        {
            // Busca a Filial correspondente
            patio.Filial = await filialCollection
                .Find(f => f.Id == patio.FilialId)
                .FirstOrDefaultAsync();

            // Busca as Motos vinculadas
            patio.Motos = await motoCollection
                .Find(m => m.PatioId == patio.Id)
                .ToListAsync();
        }

        return patios;
    }

    // READ - por ID
    public async Task<Patio?> GetByIdAsync(Guid id)
    {
        // Busca tanto pelo campo Id quanto pelo _id binário (compatível com UUID)
        var filter = Builders<Patio>.Filter.Or(
            Builders<Patio>.Filter.Eq(p => p.Id, id),
            Builders<Patio>.Filter.Eq("_id", new BsonBinaryData(id, GuidRepresentation.Standard))
        );

        var patio = await _collection.Find(filter).FirstOrDefaultAsync();
        if (patio == null) return null;

        var filialCollection = _database.GetCollection<Filial>("Filiais");
        var motoCollection = _database.GetCollection<Moto>("Motos");

        patio.Filial = await filialCollection
            .Find(f => f.Id == patio.FilialId)
            .FirstOrDefaultAsync();

        patio.Motos = await motoCollection
            .Find(m => m.PatioId == patio.Id)
            .ToListAsync();

        return patio;
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Patio patio)
    {
        var filter = Builders<Patio>.Filter.Eq(p => p.Id, patio.Id);
        var result = await _collection.ReplaceOneAsync(filter, patio);
        return result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}

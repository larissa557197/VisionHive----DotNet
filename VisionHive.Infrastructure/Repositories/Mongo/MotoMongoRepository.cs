using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using VisionHive.Domain.Entities;

namespace VisionHive.Infrastructure.Repositories.Mongo;

public class MotoMongoRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Moto> _collection;

    public MotoMongoRepository(IMongoDatabase database)
    {
        _database = database;

        // ✅ Garante compatibilidade com UUID binário padrão do Mongo
        try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch
        {
            // ignora se já tiver sido registrado
        }

        _collection = _database.GetCollection<Moto>("Motos");
    }

    // CREATE
    public async Task<Moto> CreateAsync(Moto moto)
    {
        await _collection.InsertOneAsync(moto);
        return moto;
    }

    // READ - Todos
    public async Task<List<Moto>> GetAllAsync()
    {
        var motos = await _collection.Find(_ => true).ToListAsync();

        // Inclui o Pátio e a Filial relacionados
        var patioCollection = _database.GetCollection<Patio>("Patios");
        var filialCollection = _database.GetCollection<Filial>("Filiais");

        foreach (var moto in motos)
        {
            // Busca o Pátio correspondente
            moto.Patio = await patioCollection
                .Find(p => p.Id == moto.PatioId)
                .FirstOrDefaultAsync();

            // Busca a Filial do Pátio
            if (moto.Patio != null)
            {
                moto.Patio.Filial = await filialCollection
                    .Find(f => f.Id == moto.Patio.FilialId)
                    .FirstOrDefaultAsync();
            }
        }

        return motos;
    }

    // READ - por ID
    public async Task<Moto?> GetByIdAsync(Guid id)
    {
        // Busca tanto pelo campo Id quanto pelo _id binário (compatível com UUID)
        var filter = Builders<Moto>.Filter.Or(
            Builders<Moto>.Filter.Eq(m => m.Id, id),
            Builders<Moto>.Filter.Eq("_id", new BsonBinaryData(id, GuidRepresentation.Standard))
        );

        var moto = await _collection.Find(filter).FirstOrDefaultAsync();
        if (moto == null) return null;

        var patioCollection = _database.GetCollection<Patio>("Patios");
        var filialCollection = _database.GetCollection<Filial>("Filiais");

        // Carrega o Pátio
        moto.Patio = await patioCollection
            .Find(p => p.Id == moto.PatioId)
            .FirstOrDefaultAsync();

        // Carrega a Filial do Pátio (se existir)
        if (moto.Patio != null)
        {
            moto.Patio.Filial = await filialCollection
                .Find(f => f.Id == moto.Patio.FilialId)
                .FirstOrDefaultAsync();
        }

        return moto;
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Moto moto)
    {
        var filter = Builders<Moto>.Filter.Eq(m => m.Id, moto.Id);
        var result = await _collection.ReplaceOneAsync(filter, moto);
        return result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}

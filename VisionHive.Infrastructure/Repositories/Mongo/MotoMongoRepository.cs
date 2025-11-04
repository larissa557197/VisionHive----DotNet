using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using VisionHive.Domain.Entities;

namespace VisionHive.Infrastructure.Repositories.Mongo;

public class MotoMongoRepository
{
    private readonly IMongoCollection<Moto> _collection;

    public MotoMongoRepository(IMongoDatabase? database)
    {
        /*
         * try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch
        {
            // Ignora se já foi registrado
        }

        _collection = database.GetCollection<Moto>("Motos");
         */
        if (database != null)
            _collection = database.GetCollection<Moto>("Motos");
        else
            _collection = null!;
    }
    
    // CREATE
    public virtual async Task<Moto> CreateAsync(Moto moto)
    {
        await _collection.InsertOneAsync(moto);
        return moto;
    }
    
    // READ - Todos
    public virtual async Task<List<Moto>> GetAllAsync()
    {
        return await _collection.Find(p=>true).ToListAsync();
    }
    
    // READ - por ID
    public virtual async Task<Moto?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Moto>.Filter.Or(
            Builders<Moto>.Filter.Eq(m => m.Id, id),
            Builders<Moto>.Filter.Eq("_id", new BsonBinaryData(id, GuidRepresentation.Standard))
        );

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    // UPDATE
    public virtual async Task<bool> UpdateAsync(Moto moto)
    {
        var filter = Builders<Moto>.Filter.Eq(m => m.Id, moto.Id);
        var result = await _collection.ReplaceOneAsync(filter, moto);
        return result.ModifiedCount > 0;
    }
    
    // DELETE
    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
    
}
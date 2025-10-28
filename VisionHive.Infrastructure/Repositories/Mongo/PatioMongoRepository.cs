using System.Net.Http.Headers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using VisionHive.Domain.Entities;

namespace VisionHive.Infrastructure.Repositories.Mongo;

public class PatioMongoRepository
{
    private readonly  IMongoCollection<Patio> _collection;

    public PatioMongoRepository(IMongoDatabase database)
    {
        try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch
        {
            // ignora se já foi registrado
        }
        
        _collection = database.GetCollection<Patio>("Patios");
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
        return await _collection.Find(_ => true).ToListAsync();
    }
    
    // READ - por ID
    public async Task<Patio?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Patio>.Filter.Or(
            Builders<Patio>.Filter.Eq(p => p.Id, id),
            Builders<Patio>.Filter.Eq("_id", new BsonBinaryData(id, GuidRepresentation.Standard))
            );
        
        return await _collection.Find(filter).FirstOrDefaultAsync();
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
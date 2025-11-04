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

    public PatioMongoRepository()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoSettings:DatabaseName"] ?? "VisionHiveDB";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);

        try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch { }

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

        // 🔗 Inclui Filial e Motos correspondentes
        var filialCollection = _database.GetCollection<Filial>("Filiais");
        var motoCollection = _database.GetCollection<Moto>("Motos");

        foreach (var patio in patios)
        {
            // Busca a Filial associada
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

    // READ - Por ID
    public async Task<Patio> GetByIdAsync(Guid id)
    {
        var patio = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (patio == null) return null;

        var filialCollection = _database.GetCollection<Filial>("Filiais");
        var motoCollection = _database.GetCollection<Moto>("Motos");

        // Inclui a Filial e Motos correspondentes
        patio.Filial = await filialCollection.Find(f => f.Id == patio.FilialId).FirstOrDefaultAsync();
        patio.Motos = await motoCollection.Find(m => m.PatioId == patio.Id).ToListAsync();

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


using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using VisionHive.Domain.Entities;

namespace VisionHive.Infrastructure.Repositories.Mongo;

public class FilialMongoRepository
{
    
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Filial> _collection;

    public FilialMongoRepository()
    {
        // ⚙️ Lê conexão direto do appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoSettings:DatabaseName"] ?? "VisionHiveDB";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);

        // Garante compatibilidade com UUID binário padrão do MongoDB
        try
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        }
        catch
        {
            // Ignora se já foi registrado
        }

        _collection = _database.GetCollection<Filial>("Filiais");
    }

    // CREATE
    public async Task<Filial> CreateAsync(Filial filial)
    {
        await _collection.InsertOneAsync(filial);
        return filial;
    }

    // READ - Todos
    public async Task<List<Filial>> GetAllAsync()
    {
        var filiais = await _collection.Find(_ => true).ToListAsync();

        // Inclui os pátios correspondentes
        var patioCollection = _database.GetCollection<Patio>("Patios");

        foreach (var filial in filiais)
        {
            var patios = await patioCollection
                .Find(p => p.FilialId == filial.Id)
                .ToListAsync();

            filial.Patios = patios;
        }

        return filiais;
    }

    // READ - por ID
    public async Task<Filial> GetByIdAsync(Guid id)
    {
        var filial = await _collection.Find(f => f.Id == id).FirstOrDefaultAsync();
        if (filial == null) return null;

        var patioCollection = _database.GetCollection<Patio>("Patios");
        filial.Patios = await patioCollection.Find(p => p.FilialId == id).ToListAsync();

        return filial;
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Filial filial)
    {
        var filter = Builders<Filial>.Filter.Eq(f => f.Id, filial.Id);
        var result = await _collection.ReplaceOneAsync(filter, filial);
        return result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
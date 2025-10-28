namespace VisionHive.Application.Configs;

public class Settings
{
    public SwaggerSettings Swagger { get; set; } = new();
    public MongoDbSettings MongoDb { get; set; } = new();
    public DataBaseSettings ConnectionStrings {get; set; } = new();
    
}
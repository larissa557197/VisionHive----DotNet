using Microsoft.OpenApi.Models;

namespace VisionHive.Application.Configs;

public sealed class SwaggerSettings
{
    public string Title { get; set; }
    public string Description { get; set; }
    public OpenApiContact Contact { get; set; }
    public List<OpenApiServer> Servers { get; set; }
}
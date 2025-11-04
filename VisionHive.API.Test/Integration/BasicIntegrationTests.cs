using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace VisionHive.API.Test.Integration;

public class BasicIntegrationTests : IClassFixture<WebApplicationFactory<VisionHive.API.Program>>
{
    private readonly WebApplicationFactory<VisionHive.API.Program> _factory;

    public BasicIntegrationTests(WebApplicationFactory<VisionHive.API.Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove health checks “pesados” (Mongo/Oracle) se forem adicionados por extension
                // e/ou substitua repositórios por fakes aqui se precisar no futuro.
                // Neste teste a gente só chama /health (padrão) e rotas “abertas”.
            });
        });
    }

    [Fact]
    public async Task Health_DeveRetornar_200()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task SwaggerJson_V2_DeveRetornar_200()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/swagger/v2/swagger.json");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
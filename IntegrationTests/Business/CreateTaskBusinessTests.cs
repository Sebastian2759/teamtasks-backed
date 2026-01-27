using IntegrationTests.Infra;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;

namespace IntegrationTests.Business;

public class CreateTaskBusinessTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client;

    public CreateTaskBusinessTests(TestApiFactory factory)
    {
        _client = factory.CreateClient(new() { BaseAddress = new Uri("http://localhost") });
    }

    // IDs 
    private static readonly Guid ProjectId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid DeveloperId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    // Catálogo correcto:
    // Status (StateArea) => 3000...
    private static readonly Guid StatusPorHacer = Guid.Parse("30000000-0000-0000-0000-000000000001");
    // Priority (PropertiesTask) => 4000...
    private static readonly Guid PriorityMedia = Guid.Parse("40000000-0000-0000-0000-000000000002");

    // Status incorrecto (ej: usar un ID de prioridad como si fuera status)
    private static readonly Guid WrongStatusFromPriorityCatalog = Guid.Parse("40000000-0000-0000-0000-000000000001");

    [Fact]
    public async Task CreateTask_WhenCatalogsAreValid_ShouldCreateTask()
    {
        var payload = new
        {
            projectId = ProjectId,
            title = $"Prueba negocio - OK {Guid.NewGuid()}",
            description = "Caso OK",
            assigneeId = DeveloperId,
            idTaskStatus = StatusPorHacer,
            idTaskPriority = PriorityMedia,
            estimatedComplexity = 3,
            dueDate = DateTime.UtcNow.Date.AddDays(10),
            completionDate = (DateTime?)null
        };

        var resp = await _client.PostAsJsonAsync("/api/tasks", payload);

        resp.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        var body = await resp.Content.ReadAsStringAsync();
        body.Should().Contain("title", because: "debe devolver info de la tarea creada");
    }

    [Fact]
    public async Task CreateTask_WhenStatusDoesNotBelongToStatusCatalog_ShouldReturnBusinessError()
    {
        var payload = new
        {
            projectId = ProjectId,
            title = $"Prueba negocio - FAIL {Guid.NewGuid()}",
            description = "Caso FAIL",
            assigneeId = DeveloperId,
            idTaskStatus = WrongStatusFromPriorityCatalog,
            idTaskPriority = PriorityMedia,
            estimatedComplexity = 3,
            dueDate = DateTime.UtcNow.Date.AddDays(10),
            completionDate = (DateTime?)null
        };

        var resp = await _client.PostAsJsonAsync("/api/tasks", payload);

        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await resp.Content.ReadAsStringAsync();
        body.Should().Contain("IdTaskStatus", because: "el error de negocio debe explicar el campo inválido");
    }
}

using BoatBackend.Data;
using BoatBackend.Repositories;
using BoatBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddLogging();
builder.Services.AddScoped<SetupService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BoatRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var setupService = scope.ServiceProvider.GetRequiredService<SetupService>();
    await setupService.RunSetup();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/boats", async (BoatRepository repo) =>
{
    var boats = await repo.GetAllBoats();
    return Results.Ok(boats);
}).WithName("Boats");

app.Run();
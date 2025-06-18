using System.Text;
using BoatBackend.Data;
using BoatBackend.Interfaces;
using BoatBackend.Models;
using BoatBackend.Repositories;
using BoatBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddLogging();
builder.Services.AddScoped<SetupService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<BoatRepository>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddCors(options => // NOTE in a production app this would be more restrictive!
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationService.JwtKey))
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var setupService = scope.ServiceProvider.GetRequiredService<SetupService>();
    await setupService.RunSetup();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapPost("/login", async (User user, AuthenticationService authService) =>
    {
        var token = await authService.Login(user);
        if (token == string.Empty) return Results.Unauthorized();
        return Results.Ok(token);
    })
    .WithName("Login");

app.MapGet("/ping", () => "pong").WithName("Ping");

app.MapGet("/boats", async (BoatRepository repo) => await repo.GetAllBoats()).WithName("GetBoat")
    .RequireAuthorization();

app.MapGet("/boats/{id:int}", async (BoatRepository repo, int id) => await repo.GetBoatById(id))
    .WithName("GetBoatById")
    .RequireAuthorization();

app.MapPost("/boats", async (Boat boat, BoatRepository repo) => await repo.CreateBoat(boat)).WithName("AddBoat")
    .RequireAuthorization();

app.MapDelete("/boats/{id:int}", async (BoatRepository repo, int id) => await repo.DeleteBoat(id))
    .WithName("DeleteBoat")
    .RequireAuthorization();

app.MapPut("/boats/", async (BoatRepository repo, Boat boat) => await repo.UpdateBoat(boat)).WithName("UpdateBoat")
    .RequireAuthorization();

app.Run();
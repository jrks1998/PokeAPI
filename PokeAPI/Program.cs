using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Data;
using Service;
using Serilog;
using Repository;

var builder = WebApplication.CreateBuilder(args);
var user = Environment.GetEnvironmentVariable("DB_USER_POKEAPI");
var pass = Environment.GetEnvironmentVariable("DB_PASS_POKEAPI");
var db = Environment.GetEnvironmentVariable("DB_NAME_POKEAPI");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT_POKEAPI") ?? "5432";
var connectionString = $"Host=localhost;Port={dbPort};Database={db};Username={user};Password={pass};";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<ICorPokemonRepository, CorPokemonRepository>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IConsomeApi, ConsomeApi>();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokeAPI", Version = "v1" });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeAPI v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
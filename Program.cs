using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository;
using Service;

var builder = WebApplication.CreateBuilder(args);
var user = Environment.GetEnvironmentVariable("DB_USER");
var pass = Environment.GetEnvironmentVariable("DB_PASS");
var db = Environment.GetEnvironmentVariable("DB_NAME");
var connectionString = $"Host=localhost;Port=50001;Database={db};Username={user};Password={pass};";
builder.Services.AddDbContext<PokemonRepository>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<PokemonService>();

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
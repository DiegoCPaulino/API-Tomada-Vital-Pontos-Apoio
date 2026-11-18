using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore.Infrastructure;
using PontosApoioApi.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"),
        b => b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// No .NET 8 não existe o AddOpenApi/MapOpenApi da aula (só a partir do .NET 9):
// o Swashbuckle gera o documento OpenAPI e o Scalar continua sendo a tela que o mostra.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Pontos de Apoio - Tomada Vital",
        Version = "v1",
        Description = "Locais com energia de reserva para onde a família de um paciente eletrodependente pode ir durante a queda de energia, e os recursos de cada local. Módulo .NET da Tomada Vital, com ASP.NET Core 8, EF Core e Oracle."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapScalarApiReference(options => options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração local
builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true);

// Serviços da aplicação
builder.Services.AddControllers();

// Banco de dados
builder.Services.AddDbContext<ServiceDeskContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Documentação OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Documentação OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Mapeamento dos Controllers
app.MapControllers();

app.Run();
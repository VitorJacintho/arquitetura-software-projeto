using Microsoft.EntityFrameworkCore;
using Template.Infra;
using Template.Servicos;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=alunos.db";
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<AlunosDomain>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alunos API v1");
    c.RoutePrefix = "swagger"; // acessível em /swagger
});

app.UseRouting();
app.MapControllers();
app.Run();

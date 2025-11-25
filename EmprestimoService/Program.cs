using EmprestimosService.Infra;
using EmprestimosService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("AlunosService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:AlunosService"]!);
});

builder.Services.AddHttpClient("LivrosService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:LivrosService"]!);
});

builder.Services.AddScoped<EmprestimosDomain>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

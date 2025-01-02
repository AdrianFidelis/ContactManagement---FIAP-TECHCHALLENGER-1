using ContactManagement.Domain.Repositories;
using ContactManagement.Infrastructure.Data;
using ContactManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Entity Framework Core
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do reposit�rio no cont�iner DI
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Registro dos controladores e servi�os adicionais
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

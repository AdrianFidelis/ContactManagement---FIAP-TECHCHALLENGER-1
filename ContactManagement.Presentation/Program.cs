﻿using ContactManagement.Domain.Repositories;
using ContactManagement.Infrastructure.Data;
using ContactManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using ContactManagement.Application.Validators;
using Microsoft.Extensions.Caching.Memory;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework Core
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do repositório no contêiner DI
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Adicionando suporte a In-Memory Cache
builder.Services.AddMemoryCache();

// Registra os controladores
builder.Services.AddControllers();

// Registra FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Substitui RegisterValidatorsFromAssemblyContaining<T>()
builder.Services.AddValidatorsFromAssemblyContaining<ContactValidator>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactDbContext>();

    //if (db.Database.IsRelational())
    //{
    //    db.Database.Migrate();
    //}    
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Middleware do Prometheus
app.UseHttpMetrics(); // Coleta por endpoint (GET /api/contacts, etc.)
app.MapMetrics();     // Expõe o endpoint /metrics


app.Run();

public partial class Program { }



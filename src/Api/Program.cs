using Api.Context;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyContext>(options =>
                  options.UseNpgsql(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pessoas/{id}", async (MyContext db, Guid id) => await db.Pessoas.FindAsync(id));

app.MapGet("/pessoas", async (MyContext db, [FromQuery] string t) =>
{
    PessoaEntity[] result = new PessoaEntity[] { };

    if (!String.IsNullOrEmpty(t))
    {
        result = await db.Pessoas.Where(
                    e => e.Apelido.Contains(t) ||
                    e.Nome.Contains(t) ||
                    e.Stack.Contains(t, StringComparer.OrdinalIgnoreCase)
                ).ToArrayAsync();
    }

    return result;
    
});

app.MapPost("/pessoas", async (MyContext db, [FromBody] PessoaRequest request) =>
{
    
    if (request.apelido.Length > 32)
        return Results.BadRequest();
    
    if (request.nome.Length > 100)
        return Results.BadRequest();
    
    string[] formats = new string[] { "AAAA-MM-DD" };
    if (!DateTime.TryParseExact(request.nascimento, formats, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime _validDate))
        return Results.BadRequest();
    
    PessoaEntity pessoa = new PessoaEntity() {
        Id = new Guid(),
        Apelido = request.apelido,
        Nome = request.nome,
        Nascimento = _validDate,
        Stack = request.stack
    };

    await db.Pessoas.AddAsync(pessoa);
    await db.SaveChangesAsync();

    return Results.Created($"/pessoas/{pessoa.Id}", pessoa);

});

app.MapGet("/contagem-pessoas", async (MyContext db) => await db.Pessoas.CountAsync());

app.Run();

internal record PessoaRequest(string id, string apelido, string nome, string nascimento, string[] stack);

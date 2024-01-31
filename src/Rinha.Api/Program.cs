using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Rinha.Domain.Entities;
using Rinha.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Rinha.InfraStructure.Contexts;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("ApiDatabase");
builder.Services.AddDbContext<MyContext>(options =>
                  options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

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

app.MapGet("/pessoas/{id}", async ([FromServices] IPeopleRepository repository,Guid id) =>
{
    var pessoa = await repository.GetPeople(id);
    if (pessoa == null)
        return Results.NotFound();
    else
        return Results.Ok(pessoa);
});

app.MapGet("/pessoas", async ([FromServices] IPeopleRepository repository, [FromQuery] string t) =>
{
    
    if (!string.IsNullOrEmpty(t))
    {
        var pessoas =  await repository.GetPeoples(t);
        return Results.Ok(pessoas);
    }

    return Results.Ok();
    
});

app.MapPost("/pessoas", async ([FromServices] IPeopleRepository repository, [FromBody] PeopleRequest request) =>
{
    
    if (request.apelido.Length > 32)
        return Results.BadRequest();
    
    if (request.nome.Length > 100)
        return Results.BadRequest();
    
    string[] formats = new string[] { "AAAA-MM-DD" };
    if (!DateTime.TryParseExact(request.nascimento, formats, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime _validDate))
        return Results.BadRequest();
    
    PeopleEntity pessoa = new PeopleEntity() {
        Id = new Guid(),
        Apelido = request.apelido,
        Nome = request.nome,
        Nascimento = _validDate,
        Stack = request.stack
    };

    var result = await repository.CreatePeople(pessoa);

    return Results.Created($"/pessoas/{pessoa.Id}", result);

});

app.MapGet("/contagem-pessoas", async ([FromServices] IPeopleRepository repository) => await repository.CountPeoples());

app.Run();

internal record PeopleRequest(string id, string apelido, string nome, string nascimento, string[] stack);

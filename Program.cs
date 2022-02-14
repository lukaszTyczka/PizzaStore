using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
// comand to create project
// dotnet new web -o PizzaStore -f net6.0 

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";
builder.Services.AddCors(iptions => { });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<PizzaDB>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
});
var app = builder.Build();
// comand to  install swagger.
// dotnet add package Swashbuckle.AspNetCore --version 6.1.4
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
});
app.MapGet("/", () => "Hello world!");

app.MapGet("/pizzas/{id}", async (PizzaDB db, int id) => await db.Pizzas.FindAsync(id));
app.MapGet("/pizzas", async (PizzaDB db) => await db.Pizzas.ToListAsync());

app.MapPost("/pizzas", async (PizzaDB db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

app.MapPut("/pizzas/{id}", async (PizzaDB db, Pizza updatepizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null) return Results.NotFound();
    pizza.Name = updatepizza.Name;
    pizza.Description = updatepizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/pizza/{id}", async (PizzaDB db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null)
    {
        return Results.NotFound();
    }
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.Run();

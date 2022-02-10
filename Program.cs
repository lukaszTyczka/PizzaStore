using Microsoft.OpenApi.Models;
using PizzaStore.DB;
// comand to create project
// dotnet new web -o PizzaStore -f net6.0 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(iptions => { });
builder.Services.AddEndpointsApiExplorer();
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
app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
app.MapGet("/pizzas", (int id) => PizzaDB.GetPizzas());
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
app.MapDelete("pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));


app.Run();

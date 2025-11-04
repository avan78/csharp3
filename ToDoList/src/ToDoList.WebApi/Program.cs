using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure Dependency injections
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    // addscoped používáme proto, abychom dostali vždy stejný scope
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
}
    var app = builder.Build();
{
    // Configure Middleware (Http request pipeline)
    // tj. co všechno zpracovává http requesty od klienta - autentizace, kontrola, rozdělení, přesměrování...
    app.MapControllers();
}

// app.MapGet("/nazdarSvete", () => "Nazdar světe!");





app.Run();

using ToDoList.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure Dependency injections
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
}
var app = builder.Build();
{
    // Configure Middleware (Http request pipeline)
    // tj. co všechno zpracovává http requesty od klienta - autentizace, kontrola, rozdělení, přesměrování...
    app.MapControllers();
}

// app.MapGet("/nazdarSvete", () => "Nazdar světe!");


app.Run();

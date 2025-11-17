using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure Dependency injections
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<ToDoItemsContext>();
    // addscoped používáme proto, abychom dostali vždy stejný scope
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
var app = builder.Build();
{
    // Configure Middleware (Http request pipeline)
    // tj. co všechno zpracovává http requesty od klienta - autentizace, kontrola, rozdělení, přesměrování...
    //  app.MapControllers();
    //app.UseSwagger();
    //app.UseSwaggerUI(config => config.SwaggerEndpoint("v1/swagger.json", "ToDoList API V1"));
    // builder.Services..UseSwagger.AddEndpointsApiExplorer();
    // builder.Services.AddSwaggerGen();


    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
    app.MapControllers();
}



/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();
< ItemGroup >
    < PackageReference Include = "Swashbuckle.AspNetCore" Version = "10.0.0" />
  </ ItemGroup >

   <ItemGroup>
  //  <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="10.0.0" />
 //  <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="6.9.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="6.9.0" />
 </ItemGroup>
*/

app.Run();

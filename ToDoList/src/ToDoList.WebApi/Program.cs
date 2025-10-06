var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
// první parametr = cesta, druhý = příkaz, funkce
app.MapGet("/test", () => "ahoj test!");
app.MapGet("/czechitas", () => "Hi IT!");
app.MapGet("/pozdrav/{jmeno}", (string jmeno) => $"Ahoj {jmeno}!");
app.MapGet("/scitani/{a:int}/{b:int}", (int a, int b) => $"Součet {a} + {b} = {(a + b)}");

app.MapGet("/travel", () => "Hi:Ibiza!");
app.MapGet("/nazdarSvete", () => "Nazdar světě!");


app.Run();

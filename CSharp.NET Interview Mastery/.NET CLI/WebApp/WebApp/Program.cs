var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapGet("/hello/{name}", (string name) => Results.Text(Greeter.Hello(name)));

app.Run();
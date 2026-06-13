var builder = WebApplication.CreateBuilder(args);

// Add services to the Dependency Injection (DI) container
builder.Services.AddOpenApi(); 

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Enables your API documentation in development mode
}

app.UseHttpsRedirection();

// START WRITING YOUR ENDPOINTS HERE

// app.MapGet("/hello", () => "API is running successfully!");


app.MapGet("/", () =>
{
    return "Default endpoint is running successfully!";
});


app.MapGet("/hello", () =>
{
    return "API is running successfully from hello!";
});


app.MapPost("/hello", () =>
{
    return "POST API is running successfully!";
});


app.MapPut("/hello", () =>
{
    return "PUT API is running successfully!";
});

app.MapDelete("/hello", () =>
{
    return "DELETE API is running successfully!";
});


app.Run();
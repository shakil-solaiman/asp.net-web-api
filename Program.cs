var builder = WebApplication.CreateBuilder(args);

// Add services to the Dependency Injection (DI) container
builder.Services.AddOpenApi(); 
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint exploration for OpenAPI/Swagger
builder.Services.AddSwaggerGen(); // Adds Swagger generation services


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI(); // Enable middleware to serve Swagger UI, specifying the Swagger JSON endpoint
    
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
using System.Runtime.InteropServices;


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


// Return type 01: Plain Text Response
app.MapGet("/plain-text", () =>
{
    return "API is running successfully from hello!";
});



// Return type 02: JSON Object Response
app.MapGet("/json-object", () =>
{
    var response = new {Message = "This is a Json Object response"};

    //return response;
    return Results.Ok(response); // This will return a 200 OK status code along with the JSON response
});



// Reutrn type 03: HTML Response
app.MapGet("/html", () =>
{
    return Results.Content("<h1>This is an HTML response</h1>", "text/html");
});




// Return type 03: application/json response with product details 

var products = new List<Product>
{
    //new Product {Name = "Iphone", Price = 250},

    new Product ("Samsung", 200),
    new Product ("Iphone", 250),
    new Product ("Xiaomi", 150),
    
};

app.MapGet("/product-items", () =>
{
    return Results.Ok(products);
});






// Other Request Methods

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



// Create a DTO
public record Product(string Name, decimal Price);

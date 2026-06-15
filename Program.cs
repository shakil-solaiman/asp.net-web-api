using System.Runtime.InteropServices;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

var categories = new List<Category>();


app.MapGet("/api/categories", () =>
{
    return Results.Ok(categories); 
});





app.Run();


// Create a Property

public record Category
{
    public Guid CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

}



// CRUD Operations
// Create => Create a category => POST : /api/categories
// Read => Read a category => GET : /api/categories
// Update => Update a category => PUT : /api/categories
// Delete => Delete a category => DELETE : /api/categories





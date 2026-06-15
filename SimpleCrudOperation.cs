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

// Read Categories

app.MapGet("/api/categories", () =>
{
    return Results.Ok(categories); 
});


// Create a category

app.MapPost("/api/categories", () =>
{
    var newCategory = new Category
    {
       CategoryId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
       Name = "Electronics",
       Description = "Devices and Gadgets",
       CreatedAt =DateTime.UtcNow,
    };

    categories.Add(newCategory);

    return Results.Created($"/api/categories/{newCategory.CategoryId}",newCategory);
});



// Delete a Category

app.MapDelete("/api/categories", () =>
{
    var foundCategory = categories.FirstOrDefault(categoryNum => categoryNum.CategoryId == Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));

    if(foundCategory == null)
    {
        return Results.NotFound("Category with this ID is not exist.");
    }


    categories.Remove(foundCategory);
    return Results.NoContent();
});


// Update a Category

app.MapPut("/api/categories", () =>
{
    var foundCategory = categories.FirstOrDefault(categoryNum => categoryNum.CategoryId == Guid.Parse("123e4567-e89b-12d3-a456-426614174000"));

    if(foundCategory == null)
    {
        return Results.NotFound("Category with this ID is not exist.");
    }

    foundCategory.Name = "Smart Phone";
    foundCategory.Description = "High Quality Products.";

    return Results.NoContent();
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





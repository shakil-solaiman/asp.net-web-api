using System.Buffers;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;


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

app.MapGet("/api/categories", ([FromQuery] string searchValue = "") =>
{
    Console.WriteLine(searchValue);

    if(!String.IsNullOrEmpty(searchValue))
    {
        var searchCategories = categories.Where(c => c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
        return Results.Ok(searchCategories);
    }

    return Results.Ok(categories); 
});





// Pass data by Request Body

app.MapPost("/api/categories", ([FromBody] Category categoryData) =>
{
    var newCategory = new Category
    {
       CategoryId = Guid.NewGuid(),
       Name = categoryData.Name,
       Description = categoryData.Description,
       CreatedAt =DateTime.UtcNow,
    };

    categories.Add(newCategory);
    return Results.Created($"/api/categories/{newCategory.CategoryId}",newCategory);

    // Console.WriteLine(categoryData);
    // return Results.Ok();

});






// Receive Category ID from Request Body and Delete a Category by that ID

app.MapDelete("/api/categories/{categoryID}", (Guid categoryID) =>
{
    var foundCategory = categories.FirstOrDefault(categoryNum => categoryNum.CategoryId == categoryID);

    if(foundCategory == null)
    {
        return Results.NotFound("Category with this ID is not exist.");
    }


    categories.Remove(foundCategory);
    return Results.NoContent();
});







// Receive Category ID from Request Body and Update a Category Details by that ID


app.MapPut("/api/categories/{categoryID}", (Guid categoryID, [FromBody] Category categoryData) =>
{
    var foundCategory = categories.FirstOrDefault(categoryNum => categoryNum.CategoryId == categoryID);

    if(foundCategory == null)
    {
        return Results.NotFound("Category with this ID is not exist.");
    }

    foundCategory.Name = categoryData.Name;
    foundCategory.Description = categoryData.Description;

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





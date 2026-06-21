using System;
using Microsoft.AspNetCore.Mvc;



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





// Pass data by Request Body and Create a Category 
// Set Validation - Category name can not be null


app.MapPost("/api/categories", ([FromBody] Category categoryData) =>
{

    // Category name can not be null

    if(string.IsNullOrEmpty(categoryData.Name))
    {
        return Results.BadRequest("Category name is required, it can not be empty!");
    }

    if(categoryData.Name.Length < 3)
    {
        return Results.BadRequest("Category name must be 3 character atleast");
    }



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



// {categoryID:guid} -- Route Constraints to validate routes


// Receive Category ID from Request Body and Delete a Category by that ID

app.MapDelete("/api/categories/{categoryID:guid}", (Guid categoryID) =>
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


app.MapPut("/api/categories/{categoryID:guid}", (Guid categoryID, [FromBody] Category categoryData) =>
{
    var foundCategory = categories.FirstOrDefault(categoryNum => categoryNum.CategoryId == categoryID);

    // check if category ID exist or not
    if(foundCategory == null)
    {
        return Results.NotFound("Category with this ID is not exist.");
    }


    // check category data exist or not

    if(categoryData == null)
    {
        return Results.NotFound("Category data is missing for new update.");
    }


    if(!string.IsNullOrEmpty(categoryData.Name))
    {
        if(categoryData.Name.Length > 2)
        {
            foundCategory.Name = categoryData.Name;
        }

        else
        {
            return Results.BadRequest("Category name must be 3 character atleast");
        }
    }

    if(!string.IsNullOrEmpty(categoryData.Description))
    {
        foundCategory.Description = categoryData.Description;
    }



    //foundCategory.Name = categoryData.Name ?? foundCategory.Name;  // if it does not send new value
    //foundCategory.Description = categoryData.Description ?? foundCategory.Description;  // if it does not send new value

    return Results.NoContent();
});








app.Run();


// Create a Property

public record Category
{
    public Guid CategoryId { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

}



// CRUD Operations
// Create => Create a category => POST : /api/categories
// Read => Read a category => GET : /api/categories
// Update => Update a category => PUT : /api/categories
// Delete => Delete a category => DELETE : /api/categories





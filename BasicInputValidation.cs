using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/products", (Product product) =>
{
    if (!ModelValidator.TryValidate(product, out var errors))
    {
        return Results.BadRequest(new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed.",
            Errors = errors
        });
    }

    return Results.Ok(new ApiResponse<Product>
    {
        Success = true,
        Message = "Product created successfully.",
        Data = product
    });
})
.WithName("CreateProduct")
.Produces<ApiResponse<Product>>(StatusCodes.Status200OK)
.Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest);

app.Run();

/// <summary>
/// Represents a product submitted to the API.
/// </summary>
public class Product
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Standard API response wrapper for consistent client-side handling.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

/// <summary>
/// Provides reusable model validation logic using Data Annotations.
/// </summary>
public static class ModelValidator
{
    public static bool TryValidate<T>(T model, out List<string> errors)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model!);

        bool isValid = Validator.TryValidateObject(
            model!, context, validationResults, validateAllProperties: true);

        errors = validationResults
            .Select(v => v.ErrorMessage ?? "Invalid value.")
            .ToList();

        return isValid;
    }
}
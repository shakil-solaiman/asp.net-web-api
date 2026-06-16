using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/products", (Product product) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(product);

    bool isValid = Validator.TryValidateObject(
        product, validationContext, validationResults, validateAllProperties: true);

    if (!isValid)
    {
        var errors = validationResults.Select(v => v.ErrorMessage);
        return Results.BadRequest(new { Errors = errors });
    }

    return Results.Ok(new { Message = "Product is valid!", Data = product });
});

app.Run();

public class Product
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    public string Name { get; set; }

    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}
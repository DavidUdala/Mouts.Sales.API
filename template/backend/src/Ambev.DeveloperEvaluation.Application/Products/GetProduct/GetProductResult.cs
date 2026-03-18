namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Response model for GetProduct operation
/// </summary>
public class GetProductResult
{
    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The base price of the product
    /// </summary>
    public decimal Price { get; set; }
}

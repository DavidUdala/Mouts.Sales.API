using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the list of items to be included in the sale.
    /// </summary>
    public UpdateSaleItemCommand Item { get; set; } = new();

}

/// <summary>
/// Represents a line item in the update sale command.
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the item sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the number of units of the product. Maximum of 20 identical items per sale.
    /// </summary>
    public int Quantity { get; set; }
}
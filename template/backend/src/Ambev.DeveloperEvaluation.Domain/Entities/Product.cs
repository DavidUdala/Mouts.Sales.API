using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a product available for sale in the system.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Display name of the product.
        /// Limited to 200 characters.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the product, including features or specifications.
        /// Limited to 1000 characters. Optional.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Collection of all sale line items that reference this product.
        /// </summary>
        public ICollection<SaleItem> SaleItems { get; set; } = [];
    }
}
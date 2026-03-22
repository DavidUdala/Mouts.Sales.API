using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a physical branch or store location where sales can be made.
    /// </summary>
    public class Branch : BaseEntity
    {
        /// <summary>
        /// Name of the branch (e.g., "Downtown Store", "Mall Unit 12").
        /// Limited to 200 characters.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Full physical address of the branch.
        /// Limited to 500 characters. Optional.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Collection of all sales that took place at this branch.
        /// </summary>
        public ICollection<Sale> Sales { get; set; } = [];
    }
}

using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing sale operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request containing customerId, branchId and list of items</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details including generated sale number and applied discounts</returns>
        /// <remarks>
        /// Discount rules applied per item:
        /// - Quantity below 4: no discount
        /// - Quantity 4–9: 10% discount
        /// - Quantity 10–20: 20% discount
        /// - Quantity above 20: not allowed
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves a paginated and filtered list of sales.
        /// </summary>
        /// <param name="request">
        /// Pagination and filter parameters:
        /// _page (default 1), _size (default 10), _order (e.g. "saleDate desc, totalAmount asc"),
        /// customerName (partial match), branchName (partial match), saleNumber (exact),
        /// isCancelled, minDate, maxDate, minTotal, maxTotal
        /// </param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A paginated list of sales matching the given criteria</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<GetSalesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetSalesQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            var paginatedList = new PaginatedList<GetSalesResponse> (
                _mapper.Map<List<GetSalesResponse>>(result.Items),
                result.TotalCount,
                result.Page,
                result.PageSize
            );

            return OkPaginated(paginatedList);
        }

        /// <summary>
        /// Retrieves a sale by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale (e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };

            var command = _mapper.Map<GetSaleCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<GetSaleResponse>(response));
        }

        /// <summary>
        /// Cancels a sale by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale to cancel (e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the sale was cancelled</returns>
        [HttpDelete("{id}/Cancel")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new CancelSaleRequest { Id = id };

            var command = _mapper.Map<CancelSaleCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<CancelSaleResponse>(response));
        }


        /// <summary>
        /// Updates an item within a sale (quantity and unit price)
        /// </summary>
        /// <param name="request">The update request containing the sale ID and the item to update with new quantity and unit price</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale number and ID</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSaleItem([FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<UpdateSaleResponse>(response));
        }

        /// <summary>
        /// Cancels a specific item within a sale without cancelling the entire sale.
        /// </summary>
        /// <param name="id">The unique identifier of the sale (e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6)</param>
        /// <param name="itemId">The unique identifier of the item to cancel (e.g. 7c9e6679-7425-40de-944b-e07fc1f90ae7)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale ID, sale number, item ID and cancellation status</returns>
        [HttpDelete("{id}/items/{itemId}/cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSaleItem([FromRoute] Guid id, [FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            var command = new CancelSaleItemCommand(id, itemId);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<CancelSaleItemResponse>(response));
        }
    }
}
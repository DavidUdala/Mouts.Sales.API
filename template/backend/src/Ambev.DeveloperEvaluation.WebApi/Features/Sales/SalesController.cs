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
        /// Initializes a new instance of UsersController
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
        /// <param name="request">The user creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
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
        /// <param name="request">Pagination and filter parameters</param>
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
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
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
        /// Cancel a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale to cancel</param>
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
        /// update a sale item by their ID
        /// </summary>
        /// <param name="request">Pagination and filter parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the sale was updated</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSaleItem([FromBody] UpdateSaleRequest request , CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<UpdateSaleResponse>(response));
        }

        /// <summary>
        /// Cancels a specific item within a sale.
        /// </summary>
        [HttpDelete("{id}/items/{itemId}/cancel")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSaleItem([FromRoute] Guid id, [FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            var command = new CancelSaleItemCommand(id, itemId);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
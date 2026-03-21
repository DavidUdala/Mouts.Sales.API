using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="userRepository">The user repository</param>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="bus">The Rebus bus instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IUserRepository userRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IBus bus,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _userRepository = userRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
        _bus = bus;
        _logger = logger;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var existingCustomer = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (existingCustomer == null)
            throw new InvalidOperationException($"Customer with Id {command.CustomerId} not exists");

        var existingBranch = await _branchRepository.GetByIdAsync(command.BranchId, cancellationToken);
        if (existingBranch == null)
            throw new InvalidOperationException($"Branch with Id {command.BranchId} not exists");

        var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();
        var foundProducts = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

        var missingIds = productIds.Except(foundProducts.Select(p => p.Id)).ToList();
        if (missingIds.Any())
            throw new InvalidOperationException($"Products not found: {string.Join(", ", missingIds)}");

        var sale = _mapper.Map<Sale>(command);

        sale.GenerateSaleNumber();
        sale.ApplyDiscounts();
        sale.CalculateTotals();

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        _logger.LogInformation(
            "Publishing {EventName} to message broker for Sale {SaleNumber}",
            nameof(SaleCreatedEvent),
            createdSale.SaleNumber);

        await _bus.Publish(new SaleCreatedEvent(createdSale.Id, createdSale.SaleNumber, createdSale.TotalAmount));

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}
using AutoMapper;
using MediatR;
using FluentValidation;
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
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="bus">The Rebus bus instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IBus bus,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");

        var sale = _mapper.Map<Sale>(command);

        foreach (var item in sale.Items)
        {
            item.TotalAmount = (item.Quantity * item.UnitPrice) - ((item.Quantity * item.UnitPrice) * item.Discount / 100);
        }

        sale.TotalAmount = sale.Items.Sum(i => i.TotalAmount);

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

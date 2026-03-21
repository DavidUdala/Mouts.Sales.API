using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rebus.Bus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _bus = Substitute.For<IBus>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(
            _saleRepository, _userRepository, _branchRepository,
            _productRepository, _mapper, _bus, _logger);
    }

    [Fact(DisplayName = "Given valid command When creating sale Then returns success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();
        var sale = SaleHandlerTestData.GenerateValidSale();
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber };

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(new Branch { Id = command.BranchId });
        _productRepository.GetByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(command.Items.Select(i => new Product { Id = i.ProductId }));
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When creating sale Then publishes SaleCreatedEvent")]
    public async Task Handle_ValidCommand_PublishesSaleCreatedEvent()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();
        var sale = SaleHandlerTestData.GenerateValidSale();

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(new Branch { Id = command.BranchId });
        _productRepository.GetByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(command.Items.Select(i => new Product { Id = i.ProductId }));
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _bus.Received(1).Publish(Arg.Any<object>());
    }

    [Fact(DisplayName = "Given non-existent customer When creating sale Then throws InvalidOperationException")]
    public async Task Handle_CustomerNotFound_ThrowsInvalidOperationException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.CustomerId}*");
    }

    [Fact(DisplayName = "Given non-existent branch When creating sale Then throws InvalidOperationException")]
    public async Task Handle_BranchNotFound_ThrowsInvalidOperationException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.BranchId}*");
    }

    [Fact(DisplayName = "Given valid command When creating sale Then maps command to sale entity")]
    public async Task Handle_ValidCommand_MapsCommandToSaleEntity()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateCommand();
        var sale = SaleHandlerTestData.GenerateValidSale();

        _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new User { Id = command.CustomerId });
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(new Branch { Id = command.BranchId });
        _productRepository.GetByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(command.Items.Select(i => new Product { Id = i.ProductId }));
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Sale>(command);
    }
}

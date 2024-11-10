using Common.Models;
using Common.Models.Enums;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace TicketSpotApplication.Tests.IntegrationTests.DALTests;

[TestClass]
public class CartRepositoryTests
{
    private TicketSpotDbContext _context;
    private ICartRepository _cartRepository;
    private IRepository<Customer> _customerRepository;
    private IRepository<Payment> _paymentRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<TicketSpotDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .EnableSensitiveDataLogging()
        .Options;

        _context = new TicketSpotDbContext(options);

        _customerRepository = new GenericRepository<Customer>(_context);
        _paymentRepository = new GenericRepository<Payment>(_context);

        _cartRepository = new CartRepository(_context);
    }

    [TestMethod]
    public async Task CreateAsync_VerifyCartRecordIsSuccessfullyDeleted()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com",
        };

        var payment = new Payment
        {
            Status = PaymentStatus.Pending,
            TotalAmount = 0
        };

        var expectedCart = new Cart
        {
            Id = Guid.NewGuid(),
            CartStatus = CartStatus.Active,
            CustomerId = 1,
            PaymentId = 1,
        };

        await _customerRepository.CreateAsync(customer);
        await _paymentRepository.CreateAsync(payment);

        await _cartRepository.CreateAsync(expectedCart);

        var resultingCart = await _cartRepository.GetAsync(expectedCart.Id);

        resultingCart.Should().NotBeNull();
        resultingCart.Should().BeEquivalentTo(expectedCart);
    }

    [TestMethod]
    public async Task GetAsync_VerifyCartRecordIsSuccessfullyRetrieved()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com",
        };

        var payment = new Payment
        {
            Status = PaymentStatus.Pending,
            TotalAmount = 0
        };

        var expectedCart = new Cart
        {
            Id = Guid.NewGuid(),
            CartStatus = CartStatus.Active,
            CustomerId = 1,
            PaymentId = 1,
        };

        await _customerRepository.CreateAsync(customer);
        await _paymentRepository.CreateAsync(payment);

        await _cartRepository.CreateAsync(expectedCart);

        var resultingCart = await _cartRepository.GetAsync(expectedCart.Id);

        resultingCart.Should().NotBeNull();
        resultingCart.Should().BeEquivalentTo(expectedCart);

        resultingCart.Payment.Should().NotBeNull();
        resultingCart.Payment.Should().BeEquivalentTo(payment, o => o.Excluding(f => f.Id));

        resultingCart.Customer.Should().NotBeNull();
        resultingCart.Customer.Should().BeEquivalentTo(customer, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task GetByConditionAsync_VerifyCartRecordIsSuccessfullyRetrieved()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com",
        };

        var payment = new Payment
        {
            Status = PaymentStatus.Pending,
            TotalAmount = 0
        };

        var expectedCart = new Cart
        {
            Id = Guid.NewGuid(),
            CartStatus = CartStatus.Active,
            CustomerId = 1,
            PaymentId = 1,
        };

        await _customerRepository.CreateAsync(customer);
        await _paymentRepository.CreateAsync(payment);

        await _cartRepository.CreateAsync(expectedCart);

        var resultingCart = await _cartRepository.GetByConditionAsync(
            c => c.Id == expectedCart.Id,
            c => c.Payment,
            c => c.Customer);

        resultingCart.Should().NotBeNull();
        resultingCart.Should().BeEquivalentTo(expectedCart);

        resultingCart.Payment.Should().NotBeNull();
        resultingCart.Payment.Should().BeEquivalentTo(payment, o => o.Excluding(f => f.Id));

        resultingCart.Customer.Should().NotBeNull();
        resultingCart.Customer.Should().BeEquivalentTo(customer, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task UpdateAsync_VerifyCartRecordIsSuccessfullyUpdated()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com",
        };

        var payment = new Payment
        {
            Status = PaymentStatus.Pending,
            TotalAmount = 0
        };

        var expectedCart = new Cart
        {
            Id = Guid.NewGuid(),
            CartStatus = CartStatus.Active,
            CustomerId = 1,
            PaymentId = 1,
        };

        await _customerRepository.CreateAsync(customer);
        await _paymentRepository.CreateAsync(payment);

        await _cartRepository.CreateAsync(expectedCart);

        var resultingCart = await _cartRepository.GetAsync(expectedCart.Id);

        resultingCart.Should().NotBeNull();
        resultingCart.Should().BeEquivalentTo(expectedCart);

        var updatedCart = new Cart
        {
            Id = expectedCart.Id,
            CartStatus = CartStatus.Completed,
            CustomerId = 1,
            PaymentId = 1
        };

        await _cartRepository.UpdateAsync(updatedCart);

        var finalResult = await _cartRepository.GetAsync(expectedCart.Id);

        finalResult.Should().NotBeNull();
        finalResult.Should().BeEquivalentTo(updatedCart);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

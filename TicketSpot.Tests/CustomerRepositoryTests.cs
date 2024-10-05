using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.RepositoryIntgrationTests;

[TestClass]
public class CustomerRepositoryTests
{
    private readonly ICustomerRepository _customerRepository
        = new CustomerRepository(new DataAccessLayer.TicketSpotDbContext());

    private int cleanupId;

    [TestCleanup]
    public async Task CleanUp()
    {
        await _customerRepository.DeleteAsync(cleanupId);
    }

    [TestMethod]
    public async Task Customer_Create_GetById()
    {
        var customerToCreate = new Customer
        {
            FirstName = "Paul",
            LastName = "Artreides",
            Email = "p.artreides@dune.ar"
        };

        await _customerRepository.CreateAsync(customerToCreate);

        var resultGetAll = await _customerRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(2);

        var createdId = resultGetAll.First(c => c.LastName == "Artreides").Id;

        cleanupId = createdId;

        var resultGetById = await _customerRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(customerToCreate);
    }

    [TestMethod]
    public async Task Customer_Update()
    {
        var customerToCreate = new Customer
        {
            FirstName = "Paul",
            LastName = "Artreides",
            Email = "p.artreides@dune.ar"
        };

        await _customerRepository.CreateAsync(customerToCreate);

        var resultGetAll = await _customerRepository.GetAllAsync();

        resultGetAll.Should().HaveCount(2);

        var createdId = resultGetAll.First(c => c.LastName == "Artreides").Id;

        cleanupId = createdId;

        var customerToUpdate = new Customer
        {
            Id = createdId,
            FirstName = "Paul",
            LastName = "Artreides",
            Email = "p.artreides@dune.ar"
        };

        var updateResult = await _customerRepository.UpdateAsync(customerToUpdate);

        var resultGetById = await _customerRepository.Get(c => c.Id == createdId);

        resultGetById.Should().NotBeNull();
        resultGetById.Should().BeEquivalentTo(customerToUpdate);
    }
}

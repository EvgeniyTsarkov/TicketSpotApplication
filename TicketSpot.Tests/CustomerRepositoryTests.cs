using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;

namespace TicketSpot.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private readonly ICustomerRepository _customerRepository
            = new CustomerRepository(new DataAccessLayer.TicketSpotDbContext());

        [TestMethod]
        public async Task Customer_Create()
        {
            var customerToCreate = new Customer
            {
                FirstName = "Paul",
                LastName = "Artreides",
                Email = "p.artreides@dune.ar"
            };

            await _customerRepository.CreateAsync(customerToCreate);

            var resultGetAll = await _customerRepository.GetAllAsync();

            resultGetAll.Should().HaveCount(1);

            var createdId = resultGetAll.First().Id;

            var resultGetById = await _customerRepository.Get(c => c.Id == createdId);

            resultGetById.Should().NotBeNull();
            resultGetById.Should().BeEquivalentTo(customerToCreate);

            await _customerRepository.DeleteAsync(resultGetById.Id);
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

            resultGetAll.Should().HaveCount(1);

            var createdId = resultGetAll.First().Id;

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

            await _customerRepository.DeleteAsync(resultGetById.Id);
        }
    }
}

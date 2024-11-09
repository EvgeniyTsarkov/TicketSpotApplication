using Common.Models;
using DataAccessLayer;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace TicketSpotApplication.Tests.IntegrationTests;

[TestClass]
public class GeneralRepositoryTest
{
    private TicketSpotDbContext _context;
    private IRepository<EventManager> _eventManagerRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<TicketSpotDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .EnableSensitiveDataLogging()
            .Options;

        _context = new TicketSpotDbContext(options);
        _eventManagerRepository = new GenericRepository<EventManager>(_context);
    }

    [TestMethod]
    public async Task CreateAsync_VerifyEntityIsSavedToDatabase()
    {
        var expectedResult = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(expectedResult);

        var actualResult = await _eventManagerRepository.GetAllAsync();

        actualResult.Count.Should().Be(1);
        actualResult.First().Should().BeEquivalentTo(expectedResult, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task GetAllAsync_VerifyAllEntitiesAreFetched()
    {
        var expectedResult = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(expectedResult);

        var actualResult = await _eventManagerRepository.GetAllAsync();

        actualResult.Count.Should().Be(1);
        actualResult.First().Should().BeEquivalentTo(expectedResult, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task GetAsync_VerifyEntityIsRetrievedById()
    {
        var expectedResult = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(expectedResult);

        var actualResult = await _eventManagerRepository.GetAsync(1);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task GetByConditionAsync_VerifyEntityIsRetrievedByCondition()
    {
        var expectedResult = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(expectedResult);

        var actualResult = await _eventManagerRepository.GetAllByConditionAsync(em => em.Email == "1@1.com");

        actualResult.Count.Should().Be(1);
        actualResult.First().Should().BeEquivalentTo(expectedResult, o => o.Excluding(f => f.Id));
    }

    [TestMethod]
    public async Task UpdateAsync_VerifyEntityIsUpdatedInDatabase()
    {
        var originalRecord = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(originalRecord);

        var updatedRecord = new EventManager
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "2@2.com"
        };

        var result = await _eventManagerRepository.UpdateAsync(updatedRecord);

        result.Should().BeEquivalentTo(updatedRecord, o => o.Excluding(f => f.Id));

        var actualResult = await _eventManagerRepository.GetAsync(updatedRecord.Id);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(updatedRecord);
    }

    [TestMethod]
    public async Task DeleteAsync_VerifyEntityIsDeletedFromDatabase()
    {
        var originalRecord = new EventManager
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "1@1.com"
        };

        await _eventManagerRepository.CreateAsync(originalRecord);

        var createResult = await _eventManagerRepository.GetAsync(1);

        createResult.Should().NotBeNull();
        createResult.Should().BeEquivalentTo(originalRecord, o => o.Excluding(f => f.Id));

        await _eventManagerRepository.DeleteAsync(1);

        var deleteResult = await _eventManagerRepository.GetAsync(1);

        deleteResult.Should().BeNull();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

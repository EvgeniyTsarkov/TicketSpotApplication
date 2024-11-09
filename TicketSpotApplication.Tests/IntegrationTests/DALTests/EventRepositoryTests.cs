using Common.Models;
using DataAccessLayer.Repository.Implementations;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace TicketSpotApplication.Tests.IntegrationTests.DALTests;

[TestClass]
public class EventRepositoryTests
{
    private TicketSpotDbContext _context;
    private IEventRepository _eventRepository;
    private IRepository<EventManager> _eventManagerRepository;
    private IRepository<Venue> _venueRepository;

    private readonly EventManager _eventManager = new EventManager
    {
        FirstName = "John",
        LastName = "Smith",
        Email = "1@1.com"
    };

    private readonly Venue _venue = new Venue
    {
        EventManagerId = 1,
        Address = "Madison Square, 22",
        Name = "Madison Square Garden"
    };

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<TicketSpotDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .EnableSensitiveDataLogging()
            .Options;

        _context = new TicketSpotDbContext(options);

        _eventRepository = new EventRepository(_context);

        _eventManagerRepository = new GenericRepository<EventManager>(_context);
        _venueRepository = new GenericRepository<Venue>(_context);

        _eventManagerRepository.CreateAsync(_eventManager);
        _venueRepository.CreateAsync(_venue);
    }

    [TestMethod]
    public async Task GetAllAsync_VerifyEventsAreReturnedWithIncludes()
    {
        var seededEvent = new Event
        {
            Name = "Knicks vs Lackers",
            Description = "Basketball game",
            Date = DateTime.Now,
            EventManagerId = 1,
            VenueId = 1
        };

        await _eventRepository.CreateAsync(seededEvent);

        var actualGetAllResult = await _eventRepository.GetAllAsync();

        actualGetAllResult.Should().NotBeEmpty();

        var retrievedEvent = actualGetAllResult.First();

        retrievedEvent.Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venue)
        .Excluding(f => f.EventManager));

        retrievedEvent.Venue.Should().BeEquivalentTo(_venue, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Events)
        .Excluding(f => f.EventManager));

        retrievedEvent.Venue.Events.Should().NotBeEmpty();
        retrievedEvent.Venue.Events.Count.Should().Be(1);
        retrievedEvent.Venue.Events.First().Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.EventManager)
        .Excluding(f => f.Venue));

        retrievedEvent.EventManager.Should().BeEquivalentTo(_eventManager, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venues)
        .Excluding(f => f.Events));

        retrievedEvent.EventManager.Events.Should().NotBeEmpty();
        retrievedEvent.EventManager.Events.Count.Should().Be(1);
        retrievedEvent.EventManager.Events.First().Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venue)
        .Excluding(f => f.EventManager));
    }

    [TestMethod]
    public async Task GetAsync_VerifyEventsAreReturnedWithIncludes()
    {
        var seededEvent = new Event
        {
            Name = "Knicks vs Lackers",
            Description = "Basketball game",
            Date = DateTime.Now,
            EventManagerId = 1,
            VenueId = 1
        };

        await _eventRepository.CreateAsync(seededEvent);

        var actualResult = await _eventRepository.GetAsync(1);

        actualResult.Should().NotBeNull();

        actualResult.Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venue)
        .Excluding(f => f.EventManager));

        actualResult.Venue.Should().BeEquivalentTo(_venue, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Events)
        .Excluding(f => f.EventManager));

        actualResult.Venue.Events.Should().NotBeEmpty();
        actualResult.Venue.Events.Count.Should().Be(1);
        actualResult.Venue.Events.First().Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.EventManager)
        .Excluding(f => f.Venue));

        actualResult.EventManager.Should().BeEquivalentTo(_eventManager, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venues)
        .Excluding(f => f.Events));

        actualResult.EventManager.Events.Should().NotBeEmpty();
        actualResult.EventManager.Events.Count.Should().Be(1);
        actualResult.EventManager.Events.First().Should().BeEquivalentTo(seededEvent, o => o
        .Excluding(f => f.Id)
        .Excluding(f => f.Venue)
        .Excluding(f => f.EventManager));
    }
    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

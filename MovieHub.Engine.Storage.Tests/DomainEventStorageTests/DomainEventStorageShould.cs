using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using MovieHub.Engine.Domain.DomainEvents;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Storages;
using DomainEvent = MovieHub.Engine.Domain.DomainEvents.DomainEvent;

namespace MovieHub.Engine.Storage.Tests.DomainEventStorageTests;

public class TestSimpleDomainEvent : DomainEvent
{
    public string Message { get; set; }

    public TestSimpleDomainEvent(string message, DomainEventType type = DomainEventType.InitiateMovieAddition)
    {
        Message = message;
        Type = type;
    }
}

public class DomainEventStorageShould : IClassFixture<StorageTestFixture>
{
    private readonly StorageTestFixture _fixture;
    private readonly Mock<IGuidFactory> _mockGuidFactory;
    private readonly Mock<TimeProvider> _mockTimeProvider;
    private readonly DomainEventStorage _sut;

    public DomainEventStorageShould(StorageTestFixture fixture)
    {
        _fixture = fixture;
        _mockGuidFactory = new Mock<IGuidFactory>();
        _mockTimeProvider = new Mock<TimeProvider>();
        _sut = new DomainEventStorage(fixture.GetMovieHubDbContext(), _mockGuidFactory.Object, _mockTimeProvider.Object);
    }

    [Fact]
    public async Task AddDomainEvent_WhenDomainEventHasSimpleMetaData()
    {
        await using var dbContext = _fixture.GetMovieHubDbContext();
        var testEvent = new TestSimpleDomainEvent("MetaData", DomainEventType.InitiateMovieAddition);
        var expectedId = Guid.Parse("6C814C11-4266-4311-91CE-27EF5D774F63");
        var expectedTime = new DateTimeOffset(2023, 12, 16, 14, 45, 30, TimeSpan.Zero);
        
        _mockGuidFactory.Setup(f => f.Create()).Returns(expectedId);
        _mockTimeProvider.Setup(tp => tp.GetUtcNow()).Returns(expectedTime);


        await _sut.AddEvent(testEvent, CancellationToken.None);

        var savedEvent = await dbContext.DomainEvents
            .AsQueryable()
            .FirstOrDefaultAsync(e => e.Id == expectedId);

        savedEvent.Should().NotBeNull();
        savedEvent.Id.Should().Be(expectedId);
        savedEvent.EmittedAt.Should().Be(expectedTime);
        var deserializedEvent = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TestSimpleDomainEvent>(savedEvent.Content);
        deserializedEvent.Should().BeEquivalentTo(testEvent);
        
        _mockGuidFactory.Verify(f => f.Create(), Times.Once);
        _mockTimeProvider.Verify(tp => tp.GetUtcNow(), Times.Once);
    }
}
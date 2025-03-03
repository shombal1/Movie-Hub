using Moq;
using Moq.Language.Flow;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using Quartz;

namespace MovieHub.Engine.Domain.Tests.SyncMediaViewsJobTest;

public class SyncMediaViewsJobShould
{
    private readonly SyncMediaViewsJob _sut;
    private readonly Mock<IJobExecutionContext> _jobExecutionContextMock;
    private readonly Mock<IPopMediaViewsStorage> _popMediaViewsStorageMock;
    private readonly ISetup<IGetMediaIdsStorage, Task<IEnumerable<Guid>>> _getMediaIdsStorageSetup;
    private readonly ISetup<IPopMediaViewsStorage, Task<long>> _popMediaViewsStorageSetup;
    private readonly ISetup<IPushMediaViewsStorage, Task> _pushMediaViewsStorageSetup;
    
    public SyncMediaViewsJobShould()
    {
        var getMediaIdsStorage = new Mock<IGetMediaIdsStorage>();
        _popMediaViewsStorageMock = new Mock<IPopMediaViewsStorage>();
        var pushMediaViewsStorage = new Mock<IPushMediaViewsStorage>();
        _jobExecutionContextMock = new Mock<IJobExecutionContext>();

        _jobExecutionContextMock.Setup(j => j.CancellationToken).Returns(CancellationToken.None);
        _getMediaIdsStorageSetup = getMediaIdsStorage.Setup(
            m => m.Get(It.IsAny<CancellationToken>()));
        _popMediaViewsStorageSetup = _popMediaViewsStorageMock.Setup(
            m => m.PopViews(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _pushMediaViewsStorageSetup = pushMediaViewsStorage.Setup(m =>
            m.Increment(It.IsAny<IEnumerable<(Guid, long)>>(), It.IsAny<CancellationToken>()));

        _sut = new SyncMediaViewsJob(
            getMediaIdsStorage.Object,
            _popMediaViewsStorageMock.Object,
            pushMediaViewsStorage.Object);
    }
    
    [Fact]
    public async Task ReturnSuccess_WhenCallPopViewsForEachMediaId()
    {
        List<Guid> mediaIds =
            [Guid.Parse("AFAF8F84-1D49-4093-B9DA-4828F2743F04"), Guid.Parse("6620B156-95FB-467D-8864-1C31F61FA828")];
        var cancellationToken = new CancellationToken();
        _jobExecutionContextMock.Setup(m => m.CancellationToken).Returns(cancellationToken);
        _getMediaIdsStorageSetup.ReturnsAsync(mediaIds);
        _popMediaViewsStorageSetup.ReturnsAsync(5);
        
        await _sut.Execute(_jobExecutionContextMock.Object);
        
        foreach (var mediaId in mediaIds)
        {
            _popMediaViewsStorageMock.Verify(m => m.PopViews(mediaId, cancellationToken), Times.Once);
        }
    }
    
    
}
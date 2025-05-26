using System.Text;

namespace MovieHub.Engine.Storage.Tests.S3FileUploadServiceTests;

public class FakeDataStream : Stream
{
    private readonly int _size;
    private readonly byte[] _pattern;
    private long _position;

    public FakeDataStream(int size, int partNumber)
    {
        _size = size;
        _position = 0;
        _pattern = Encoding.ASCII.GetBytes($"Part-{partNumber}-Data-");
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => _size;

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_position >= _size)
            return 0;

        int bytesToReturn = (int)Math.Min(count, _size - _position);

        for (int i = 0; i < bytesToReturn; i++)
        {
            buffer[offset + i] = _pattern[(int)(_position + i) % _pattern.Length];
        }

        _position += bytesToReturn;
        return bytesToReturn;
    }

    public override void Flush()
    {
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}
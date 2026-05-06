namespace Example.API.Common.Services.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTime BangkokNow { get; }
}
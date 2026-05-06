using System.Runtime.InteropServices;
using Example.API.Common.Services.Interfaces;

namespace Example.API.Common.Services;

public class TimeProvider : ITimeProvider
{
    private readonly TimeZoneInfo _bangkokTimeZone;

    public TimeProvider()
    {
        _bangkokTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "SE Asia Standard Time"
                : "Asia/Bangkok"
        );
    }

    public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _bangkokTimeZone);
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime BangkokNow => Now;
}
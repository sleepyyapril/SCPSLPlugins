using System;

namespace Speedrun.Helpers;

public class Stopwatch
{
    private TimeSpan _start;
    
    public void Start()
    {
        _start = DateTimeOffset.UtcNow.TimeOfDay;
    }

    public TimeSpan ElapsedTime()
    {
        return DateTimeOffset.UtcNow.TimeOfDay - _start;
    }
}
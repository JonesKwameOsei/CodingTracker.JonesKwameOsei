using System.Diagnostics;

namespace CodingTracker.JonesKwameOsei;

internal class LiveSession
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _pausedAccumulated = TimeSpan.Zero;
    private DateTime? _end;
    private bool _stopped;

    internal string Language { get; }
    internal DateTime StartTime { get; } = DateTime.Now;
    internal DateTime EndTime => _end ?? StartTime + _stopwatch.Elapsed;
    internal bool IsPaused { get; private set; }
    internal bool IsFinished => _stopped;
    internal bool Saved { get; private set; }
    internal List<TimeSpan> Laps { get; } = new();

    internal TimeSpan Elapsed => _stopwatch.Elapsed;

    internal LiveSession(string language)
    {
        Language = language;
        _stopwatch.Start();
    }

    internal void Pause()
    {
        if (_stopped || IsPaused) return;
        _stopwatch.Stop();
        IsPaused = true;
    }

    internal void Resume()
    {
        if (_stopped || !IsPaused) return;
        _stopwatch.Start();
        IsPaused = false;
    }

    internal void AddLap()
    {
        if (_stopped) return;
        Laps.Add(_stopwatch.Elapsed);
    }

    internal void Stop(bool save)
    {
        if (_stopped) return;
        if (IsPaused) _stopwatch.Start(); // ensure final elapsed reflects only active time
        _stopwatch.Stop();
        _end = StartTime + _stopwatch.Elapsed;
        _stopped = true;
        Saved = save;
    }
}

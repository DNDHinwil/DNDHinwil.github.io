namespace DNDHinwil.Website.Models;

public class UpCountingTimer : DNDHinwil.Website.Interfaces.ITimer
{
    private readonly Timer _timer;
    private TimeSpan _runTime;
    private readonly long _timeBetweenTicksInMilliseconds = 1000;
    private string _timeFormat => ShowHours ? @"hh\:mm\:ss" : @"mm\:ss";
    private TimerCallback? _callback;

    public bool TimerRunning { get; set; }
    public bool Paused { get; set; }
    public bool ShowHours { get; set; }
    public string TimeString => _runTime.ToString(_timeFormat);
    public TimeSpan Time => _runTime;

    public UpCountingTimer(TimerCallback? callback)
    {
        _timer = new Timer(Tick, null, _timeBetweenTicksInMilliseconds, _timeBetweenTicksInMilliseconds);
        _callback = callback;
    }

    public void StartTimer()
    {
        if (TimerRunning)
            return;
        _runTime = TimeSpan.Zero;
        TimerRunning = true;
        UnpauseTimer();
    }

    public void PauseTimer() => Paused = true;

    public void UnpauseTimer() => Paused = false;

    public void StopTimer()
    {
        UnpauseTimer();
        TimerRunning = false;
    }

    private void Tick(object? state)
    {
        if (!TimerRunning || Paused)
            return;
        _runTime = _runTime.Add(TimeSpan.FromMilliseconds(_timeBetweenTicksInMilliseconds));
        _callback?.Invoke(null);
    }
}

namespace DNDHinwil.Website.Interfaces;

public interface ITimer
{
    public TimeSpan Time { get; }
    string TimeString { get; }
    bool TimerRunning { get; set; }
    bool Paused { get; set; }
    bool ShowHours { get; set; }

    void PauseTimer();
    void UnpauseTimer();
    void StartTimer();
    void StopTimer();
}
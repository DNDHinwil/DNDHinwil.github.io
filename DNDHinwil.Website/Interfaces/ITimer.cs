namespace DNDHinwil.Website.Models;

public interface ITimer
{
    string Time { get; }
    bool TimerRunning { get; set; }
    bool Paused { get; set; }
    bool ShowHours { get; set; }

    void PauseTimer();
    void UnpauseTimer();
    void StartTimer();
    void StopTimer();
}
using System;

namespace SleepyGameModeAPI.CustomEventArgs;

public class GameModeChangedEventArgs : EventArgs
{
    public GameModeChangedEventArgs(
        SleepyGameMode? oldGameMode,
        SleepyGameMode? newGameMode)
    {
        OldGameMode = oldGameMode;
        NewGameMode = newGameMode;
    }

    public SleepyGameMode? OldGameMode { get; set; }
    public SleepyGameMode? NewGameMode { get; set; }
}
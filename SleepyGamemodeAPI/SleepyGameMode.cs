using LabApi.Loader.Features.Plugins;

namespace SleepyGameModeAPI;

public abstract class SleepyGameMode
{
    public abstract Plugin GameModePlugin { get; }
    public virtual int PickWeight => 1;

    public abstract void Initialize();
    public abstract void Dispose();
    public abstract void Started();
    public abstract void Stopped();
}
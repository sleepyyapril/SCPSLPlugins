using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;
using SleepyGameModeAPI;

namespace Murder;

public class MurderGameMode(MurderPlugin plugin) : SleepyGameMode
{
    public override void Initialize()
    {
        
    }

    public override void Dispose()
    {
        
    }

    public override void Started()
    {
        CustomHandlersManager.RegisterEventsHandler(plugin.MurderEvents);
    }

    public override void Stopped()
    {
        CustomHandlersManager.UnregisterEventsHandler(plugin.MurderEvents);
    }

    public override Plugin GameModePlugin => plugin;
}
using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using SleepyGameModeAPI.Core;
using SleepyGameModeAPI.Managers;

namespace SleepyGameModeAPI;

public class SleepyGameModeAPI : Plugin<SleepyGameModeConfig>
{
    public override string Name => "SleepyGameModeAPI";
    public override string Description => "A simple gamemode API.";
    public override string Author => "sleepyyapril";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion { get; } = new (LabApiProperties.CompiledVersion);
    public override LoadPriority Priority => LoadPriority.Highest;

    public GameModeManager GameModeManager = null!;
    private SleepyGameModeEvents _events = null!;

    public override void Enable()
    {
        _events = new SleepyGameModeEvents(this);
        GameModeManager = new GameModeManager();
        
        CustomHandlersManager.RegisterEventsHandler(_events);
    }

    public override void Disable()
    {
        GameModeManager.Dispose();
        CustomHandlersManager.UnregisterEventsHandler(_events);
    }
}

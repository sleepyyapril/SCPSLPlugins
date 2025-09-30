using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Speedrun.Core;
using Speedrun.Data;
using Speedrun.Enums;

namespace Speedrun;

public class SpeedrunPlugin : Plugin<Config>
{
    public override string Name { get; } = "Speedrun";
    public override string Description { get; } = "A plugin for tracking speedrun statistics.";
    public override string Author { get; } = "sleepyyapril";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion { get; } = new (LabApiProperties.CompiledVersion);
    
    public SaveDataManager SaveDataManager = null!;
    public TimesManager TimesManager = null!;
    
    private Events _events = null!;

    public override void Enable()
    {
        _events = new Events(this);
        SaveDataManager = new SaveDataManager(this);
        TimesManager = new TimesManager(this);
        
        SaveDataManager.Initialize();
        CustomHandlersManager.RegisterEventsHandler(_events);
    }

    public override void Disable()
    {
        SaveDataManager.Dispose();
        CustomHandlersManager.UnregisterEventsHandler(_events);
    }
}

public record struct SpeedrunData(SpeedrunType Speedrun, long Elapsed)
{
    public SpeedrunType Speedrun = Speedrun;
    public long Elapsed = Elapsed;
}
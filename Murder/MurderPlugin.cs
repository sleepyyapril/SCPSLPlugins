using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Murder.Core;
using SleepyGameModeAPI.Managers;

namespace Murder;

public class MurderPlugin : Plugin<MurderConfig>
{
    public override string Name { get; } = "Murder";
    public override string Description { get; } = "A GMod Murder gamemode!";
    public override string Author { get; } = "sleepyyapril";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion { get; } = new (LabApiProperties.CompiledVersion);

    public MurderGameMode MurderGameMode = null!;
    public MurderEvents MurderEvents = null!;

    public override void Enable()
    {
        MurderEvents = new MurderEvents(this);
        MurderGameMode = new MurderGameMode(this);
        
        GameModeManager.RegisterGameMode(MurderGameMode);
    }

    public override void Disable()
    {
        GameModeManager.UnregisterGameMode(MurderGameMode);
    }
}

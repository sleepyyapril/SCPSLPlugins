using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;

namespace CoinTricks;

public class CoinTricks : Plugin
{
    public override string Name { get; } = "CoinTricks";
    public override string Description { get; } = "Random RNG-based tricks for coins.";
    public override string Author { get; } = "sleepyyapril";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

    public Events Events;
    public Config Config;
    public TricksHandler TricksHandler;

    private bool _invalidConfig;

    public override void Enable()
    {
        if (_invalidConfig)
        {
            Logger.Error("CoinTricks has an invalid configuration. Plugin will not run.");
            return;
        }
        
        Events = new Events(this);
        TricksHandler = new TricksHandler(this);
        
        TricksHandler.RegisterTricks();
        CustomHandlersManager.RegisterEventsHandler(Events);
    }

    public override void Disable()
    {
        CustomHandlersManager.UnregisterEventsHandler(Events);
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        _invalidConfig = !this.TryLoadConfig("cointricks.yml", out Config);
    }

    public bool CanRunTrick(string trickName)
    {
        // I don't want to type "trick" at the end every time
        var containsTrick = Config.DisabledTricks.Contains(trickName);
        
        // If InvertDisabledTricks is true, treat it as a trick whitelist
        // Otherwise, it's a blacklist.
        return Config.InvertDisabledTricks switch
        {
            true when containsTrick => true,
            false when !containsTrick => true,
            _ => false
        };
    }
}
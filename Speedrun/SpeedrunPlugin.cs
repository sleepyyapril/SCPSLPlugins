using System.IO;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using Speedrun.Core;
using Speedrun.Enums;
using Speedrun.Handlers;
using Version = System.Version;

namespace Speedrun;

public class SpeedrunPlugin : Plugin<Config>
{
    public override string Name { get; } = "Speedrun";
    public override string Description { get; } = "A plugin for tracking speedrun statistics.";
    public override string Author { get; } = "sleepyyapril";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion { get; } = new (LabApiProperties.CompiledVersion);
    
    public SaveDataHandler SaveDataHandler = null!;
    public TimesHandler TimesHandler = null!;
    
    private StopwatchHandler _stopwatchHandler = null!;
    private Events _events = null!;

    public override void Enable()
    {
        _events = new Events(this);
        _stopwatchHandler = new StopwatchHandler(this);
        
        SaveDataHandler = new SaveDataHandler(this);
        TimesHandler = new TimesHandler(this);
        SaveDataHandler.Initialize();
        
        CustomHandlersManager.RegisterEventsHandler(_events);
        CustomHandlersManager.RegisterEventsHandler(_stopwatchHandler);
    }

    public override void Disable()
    {
        SaveDataHandler.Dispose();
        
        CustomHandlersManager.UnregisterEventsHandler(_events);
        CustomHandlersManager.UnregisterEventsHandler(_stopwatchHandler);
    }

    public string GetDataFolder()
    {
        var dataFolder = this.GetConfigDirectory(Config?.UseGlobalDatabase ?? false);
        return dataFolder.FullName;
    }

    public string GetPathForFile(string fileName)
    {
        var dataFolder = GetDataFolder();
        return Path.Combine(dataFolder, fileName);
    }

    public string GetDatabaseFile()
    {
        if (Config == null)
            return "speedrun.db";

        return Config.DatabaseFile;
    }
}

public record struct SpeedrunData(SpeedrunType Speedrun, long Elapsed)
{
    public SpeedrunType Speedrun = Speedrun;
    public long Elapsed = Elapsed;
}
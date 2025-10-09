#nullable enable
using LabApi.Events.CustomHandlers;

namespace Speedrun.Handlers;

public class StopwatchHandler(SpeedrunPlugin plugin) : CustomEventsHandler
{
    private SpeedrunPlugin _plugin = plugin;
}
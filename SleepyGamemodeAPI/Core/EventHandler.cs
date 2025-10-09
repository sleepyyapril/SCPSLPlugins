using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using SleepyGameModeAPI.Managers;
using Random = UnityEngine.Random;

namespace SleepyGameModeAPI.Core;

public class EventHandler(SleepyGameModeAPI plugin) : CustomEventsHandler
{
    public override void OnServerWaitingForPlayers()
    {
        if (!plugin.Config!.RoundStartGameModes || plugin.Config!.PickPercentage <= 0
            || !GameModeManager.HasGameModes())
            return;
        
        if (plugin.Config!.PickPercentage >= 100)
            plugin.GameModeManager.PickGameMode();

        var random = Random.Range(1, 100);
        
        if (random < plugin.Config.PickPercentage)
            return;
        
        plugin.GameModeManager.PickGameMode();
    }
}
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using Speedrun.Enums;
using Speedrun.Helpers;

namespace Speedrun.Core;

public class Events(SpeedrunPlugin plugin) : CustomEventsHandler
{
    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (ev.Player.DoNotTrack)
        {
            plugin.SaveDataHandler.EnforceDoNotTrack(ev.Player);
            return;
        }
        
        plugin.TimesHandler.LoadPlayerTime(ev.Player);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        if (ev.Player.DoNotTrack)
        {
            plugin.SaveDataHandler.EnforceDoNotTrack(ev.Player);
            return;
        }
        
        plugin.TimesHandler.SavePlayerTime(ev.Player);
    }

    // Round actually starting is a good metric for who'll be in round.
    public override void OnServerRoundStarted()
    {
        plugin.TimesHandler.ClearOldCache();
    }
}
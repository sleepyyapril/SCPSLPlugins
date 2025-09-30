using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using Speedrun.Enums;
using Speedrun.Helpers;

namespace Speedrun.Core;

public class Events(SpeedrunPlugin plugin) : CustomEventsHandler
{
    private Stopwatch _roundStartStopwatch = new();

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (ev.Player.DoNotTrack)
        {
            plugin.SaveDataManager.EnforceDoNotTrack(ev.Player);
            return;
        }
        
        plugin.TimesManager.LoadPlayerTime(ev.Player);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        if (ev.Player.DoNotTrack)
        {
            plugin.SaveDataManager.EnforceDoNotTrack(ev.Player);
            return;
        }
        
        plugin.TimesManager.SavePlayerTime(ev.Player);
    }

    // Round actually starting is a good metric for who'll be in round.
    public override void OnServerRoundStarted()
    {
        _roundStartStopwatch.Start();
        plugin.TimesManager.ClearOldCache();
    }

    public override void OnPlayerEscaped(PlayerEscapedEventArgs ev)
    {
        if (ev.Player.DoNotTrack)
            return;
        
        var elapsed = _roundStartStopwatch.ElapsedTime();
        plugin.TimesManager.SetTime(SpeedrunType.Escape, ev.Player, (long) elapsed.TotalMilliseconds);
        ev.Player.SendBroadcast($"Escaped: {elapsed:mm':'ss'.'fff}", 3);
    }
}
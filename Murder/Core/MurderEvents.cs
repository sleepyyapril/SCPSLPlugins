using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace Murder.Core;

public class MurderEvents(MurderPlugin plugin) : CustomEventsHandler
{
    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        ev.Player.SendConsoleMessage("Player joined.");
    }
}
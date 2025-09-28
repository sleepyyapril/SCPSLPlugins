using System;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;

namespace CoinTricks.Core;

public class Events(CoinTricks instance) : CustomEventsHandler
{
    private CoinTricks _instance = instance;

    public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev)
    {
        var trick = _instance.TricksHandler.PickRandomTrick();
        var args =  new TrickArgs(ev);

        if (trick == null)
        {
            Logger.Warn("Couldn't find a trick.");
            return;
        }
        
        trick.Execute(args);
    }
}
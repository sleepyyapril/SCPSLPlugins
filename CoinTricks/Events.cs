using System;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace CoinTricks;

public class Events(CoinTricks instance) : CustomEventsHandler
{
    private CoinTricks _instance = instance;
    private Random  _random = new Random();

    public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev)
    {
        var trick = _instance.TricksHandler.PickRandomTrick();
        var args =  new TrickArgs(ev); 
        trick.Execute(args);
    }
}
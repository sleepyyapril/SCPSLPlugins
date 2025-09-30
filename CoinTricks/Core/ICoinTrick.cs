#nullable enable
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;

namespace CoinTricks.Core;

public interface ICoinTrick
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Weight { get; }
    public abstract bool Execute(TrickArgs args);
}

public struct TrickArgs(PlayerFlippedCoinEventArgs ev)
{
    public Player Player = ev.Player;
    public CoinItem CoinItem = ev.CoinItem;
    public bool IsTails = ev.IsTails;
}
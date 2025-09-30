#nullable enable
using CoinTricks.Core;

namespace CoinTricks.Tricks;

public class KillTrick : ICoinTrick
{
    public string Name { get; } = "kill";
    public string Description { get; } = "Kills the user.";
    public int Weight { get; } = 1;
    
    public bool Execute(TrickArgs args)
    {
        args.Player.Kill();
        return true;
    }
}
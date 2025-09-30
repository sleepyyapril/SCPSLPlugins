#nullable enable
using CoinTricks.Core;

namespace CoinTricks.Tricks;

public class NothingTrick : ICoinTrick
{
    public string Name { get; } = "nothing";
    public string Description { get; } = "Do nothing.";
    public int Weight { get; } = 5;
    
    public bool Execute(TrickArgs args)
    {
        return true;
    }
}
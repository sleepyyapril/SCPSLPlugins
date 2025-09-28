namespace CoinTricks.Tricks;

public class ExplodeTrick : ICoinTrick
{
    public string Name { get; } = "Explode";
    public string Description { get; } = "Explodes the user.";
    public bool Execute(TrickArgs args)
    {
        args.CoinItem.Base.
    }
}
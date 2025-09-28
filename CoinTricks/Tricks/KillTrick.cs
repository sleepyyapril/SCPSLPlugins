namespace CoinTricks.Tricks;

public class KillTrick : ICoinTrick
{
    public string Name { get; } = "Kill";
    public string Description { get; } = "Kills the user.";
    public bool Execute(TrickArgs args)
    {
        args.Player.Kill();
        return true;
    }
}
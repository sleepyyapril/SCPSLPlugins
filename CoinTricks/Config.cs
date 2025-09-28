using System.Collections.Generic;

namespace CoinTricks;

public class Config
{
    public bool InvertDisabledTricks  { get; set; } = false;
    public List<string> DisabledTricks { get; set; } = [];
}
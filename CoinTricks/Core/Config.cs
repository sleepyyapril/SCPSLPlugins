#nullable enable
using System.Collections.Generic;

namespace CoinTricks.Core;

public class Config
{
    public bool InvertDisabledTricks  { get; set; } = false;
    public List<string> DisabledTricks { get; set; } = [];
    public Dictionary<string, int> CustomWeights = new();
}
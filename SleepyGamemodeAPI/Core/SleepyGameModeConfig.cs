using System.Collections.Generic;
using System.ComponentModel;

// ReSharper disable CollectionNeverUpdated.Global

namespace SleepyGameModeAPI.Core;

public class SleepyGameModeConfig
{
    [Description("Should a game mode be picked in lobby?")]
    public bool RoundStartGameModes { get; set; } = true;
    
    [Description("What is the chance of any game mode being picked?")]
    public int PickPercentage { get; set; } = 100;
    
    [Description("A list of disabled game modes by plugin name.")]
    public List<string> DisabledGameModes { get; set; } = new();
}
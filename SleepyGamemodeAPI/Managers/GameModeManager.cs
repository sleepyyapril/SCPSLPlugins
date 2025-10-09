using System.Collections.Generic;
using System.Linq;
using SleepyGameModeAPI.Extensions;
// ReSharper disable MemberCanBePrivate.Global

namespace SleepyGameModeAPI.Managers;

public class GameModeManager
{
    private static SleepyGameMode? _activeGameMode;
    
    private static Dictionary<string, SleepyGameMode> _gameModes = new();
    private static Dictionary<SleepyGameMode, int> _gameModeWeights = new();

    internal void Dispose()
    {
        StopGameMode();
        
        foreach (var gameMode in _gameModes.Values)
        {
            gameMode.Dispose();
        }
    }

    public static void RegisterGameMode(SleepyGameMode gameMode)
    {
        var name = gameMode.GameModePlugin.Name.ToLower();
        
        if (_gameModes.ContainsKey(name))
            return;
        
        _gameModes.Add(name, gameMode);
    }

    public static void UnregisterGameMode(SleepyGameMode gameMode)
    {
        var name = gameMode.GameModePlugin.Name.ToLower();
        
        if (!_gameModes.Remove(name))
            return;
        
        gameMode.Dispose();
    }

    public static bool HasGameModes()
    {
        return _gameModes.Any();
    }

    public static void StartGameMode(string gameModeName)
    {
        gameModeName = gameModeName.ToLower();
        
        if (!_gameModes.TryGetValue(gameModeName, out var gameMode))
            return;

        StartGameMode(gameMode);
    }
    
    public static void StartGameMode(SleepyGameMode gameMode)
    {
        StopGameMode();
        
        _activeGameMode = gameMode;
        gameMode.Started();
    }

    public static void StopGameMode()
    {
        if (_activeGameMode == null)
            return;
        
        _activeGameMode.Stopped();
        _activeGameMode = null;
    }

    public static void StopGameMode(string gameModeName)
    {
        gameModeName = gameModeName.ToLower();
        
        if (!_gameModes.TryGetValue(gameModeName, out var gameMode))
            return;

        StopGameMode(gameMode);
    }
    
    public static void StopGameMode(SleepyGameMode gameMode)
    {
        if (_activeGameMode == gameMode)
        {
            StopGameMode();
            return;
        }
        
        gameMode.Stopped();
    }

    internal void PickGameMode()
    {
        StopGameMode();
        
        if (_gameModeWeights.Count != _gameModes.Count)
            BuildWeights();

        var gameMode = _gameModeWeights.GetRandomWeight();
        StartGameMode(gameMode);
    }

    private void BuildWeights()
    {
        if (_gameModes.Count == 0)
            return;

        _gameModeWeights.Clear();
        
        foreach (var gameMode in _gameModes.Values)
        {
            _gameModeWeights.Add(gameMode, gameMode.PickWeight);
        }
    }
}
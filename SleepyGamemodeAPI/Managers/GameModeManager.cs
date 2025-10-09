using System;
using System.Collections.Generic;
using System.Linq;
using SleepyGameModeAPI.Core;
using SleepyGameModeAPI.CustomEventArgs;
using SleepyGameModeAPI.Extensions;
// ReSharper disable MemberCanBePrivate.Global

namespace SleepyGameModeAPI.Managers;

public class GameModeManager
{
    public static event Action<GameModeChangedEventArgs>? OnGameModeChanged; 
    
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
    
    public static void StartGameMode(SleepyGameMode gameMode, bool invokeEvent = true)
    {
        if (invokeEvent)
        {
            var args = new GameModeChangedEventArgs(_activeGameMode, gameMode);
            OnGameModeChanged?.Invoke(args);
        }
        
        StopGameMode(false);
        
        _activeGameMode = gameMode;
        gameMode.Started();
    }

    public static void StopGameMode(bool invokeEvent = true)
    {
        if (_activeGameMode == null)
            return;

        if (invokeEvent)
        {
            var args = new GameModeChangedEventArgs(_activeGameMode, null);
            OnGameModeChanged?.Invoke(args);
        }
        
        _activeGameMode.Stopped();
        _activeGameMode = null;
    }

    internal void PickGameMode()
    {
        if (_gameModeWeights.Count != _gameModes.Count)
            BuildWeights();

        var gameMode = _gameModeWeights.GetRandomWeight();
        StopGameMode(false);
        
        var args = new GameModeChangedEventArgs(_activeGameMode, gameMode);
        OnGameModeChanged?.Invoke(args);
        
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
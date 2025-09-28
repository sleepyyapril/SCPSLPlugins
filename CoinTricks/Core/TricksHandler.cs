using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoinTricks.Extensions;
using LabApi.Features.Console;

namespace CoinTricks.Core;

public class TricksHandler(CoinTricks plugin)
{
    private readonly Dictionary<ICoinTrick, int> _tricks = [];

    private void RegisterTrick(ICoinTrick trick)
    {
        var weight = trick.Weight;
        
        if (plugin.Config.CustomWeights.TryGetValue(trick.Name, out var customWeight))
            weight = customWeight;
        
        _tricks.Add(trick, weight);
        Logger.Debug($"Registered trick `{trick.Name}` with weight `{weight}`");
    }
    
    public void RegisterTricks()
    {
        _tricks.Clear();
        
        var type = typeof(ICoinTrick);
        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
            .Select(t => (ICoinTrick) Activator.CreateInstance(t))
            .Where(p => plugin.CanRunTrick(p.Name));

        foreach (var handler in handlers)
        {
            if (handler.Name == null)
                continue;
            
            RegisterTrick(handler);
        }
    }

    public ICoinTrick PickRandomTrick()
    {
        var element = _tricks.GetRandomWeight();
        return element;
    }
}
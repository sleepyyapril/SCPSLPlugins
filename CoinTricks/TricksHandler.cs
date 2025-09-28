using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Console;

namespace CoinTricks;

public class TricksHandler(CoinTricks instance)
{
    private readonly HashSet<ICoinTrick> _tricks = [];
    private Random _random;

    private void RegisterTrick(ICoinTrick trick)
    {
        _tricks.Add(trick);
    }
    
    public void RegisterTricks()
    {
        _tricks.Clear();
        
        var type = typeof(ICoinTrick);
        var handlers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
            .Select(t => (ICoinTrick) Activator.CreateInstance(t))
            .Where(p => instance.CanRunTrick(p.Name));

        foreach (var handler in handlers)
        {
            if (handler.Name == null)
                continue;
            
            RegisterTrick(handler);
        }

        _random = new Random();
    }

    public ICoinTrick PickRandomTrick()
    {
        var index = _random.Next(0, _tricks.Count - 1);
        return _tricks.ElementAt(index);
    }
}
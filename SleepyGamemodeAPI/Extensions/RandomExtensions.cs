using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace SleepyGameModeAPI.Extensions;

public static class RandomExtensions
{
    public static T GetRandomWeight<T>(this Dictionary<T, int> dictionary, T? defaultValue = default)
    {
        var sum = dictionary.Values.Sum();
        var chance = Random.Range(1, sum + 1);
        var returned = defaultValue;

        foreach (var pair in dictionary)
        {
            if (chance <= pair.Value)
            {
                returned = pair.Key;
                break;
            }
            
            chance -= pair.Value;
        }

        return returned!;
    }
}
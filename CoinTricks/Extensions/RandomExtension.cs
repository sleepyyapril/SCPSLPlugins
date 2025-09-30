#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoinTricks.Extensions;

// Taken and edited from https://github.com/KadavasKingdom/LabApiExtensions/blob/main/LabApiExtensions/Extensions/RNGExtension.cs
public static class RandomExtension
{
    /// <summary>
    /// Getting a random element from the <paramref name="dictionary"/>, if it wasn't able to get one it uses the <paramref name="defaultValue"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetRandomWeight<T>(this Dictionary<T, int> dictionary, T defaultValue = default)
    {
        var sum = dictionary.Values.Sum();
        var chance = Random.Range(1, sum + 1);
        var returnT = defaultValue;

        foreach (var pair in dictionary)
        {
            if (chance <= pair.Value)
            {
                returnT = pair.Key;
                break;
            }
            
            chance -= pair.Value;
        }

        return returnT;
    }
}
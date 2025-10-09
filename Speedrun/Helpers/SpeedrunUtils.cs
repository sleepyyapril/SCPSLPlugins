#nullable enable
using System.Collections.Generic;
using Speedrun.Enums;

namespace Speedrun.Helpers;

public class SpeedrunUtils
{
    public static SpeedrunData GetSpeedrunData(SpeedrunType speedrun, long elapsed)
    {
        return new SpeedrunData(speedrun, elapsed);
    }
    
    public static List<SpeedrunData> GetDictionaryAsDataList(Dictionary<SpeedrunType, long> dictionary)
    {
        var dataList = new List<SpeedrunData>();

        foreach (var pair in dictionary)
        {
            var data = GetSpeedrunData(pair.Key, pair.Value);
            dataList.Add(data);
        }
        
        return dataList;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using Speedrun.Enums;
using Speedrun.Helpers;
using Utils.NonAllocLINQ;

namespace Speedrun.Handlers;

public class TimesHandler(SpeedrunPlugin plugin)
{
    private readonly Dictionary<string, List<SpeedrunData>> _times = new();

    public void LoadPlayerTime(Player player)
    {
        if (_times.ContainsKey(player.UserId))
            return;
        
        var times = plugin.SaveDataHandler.LoadPlayerTimes(player);
        var speedrunData = SpeedrunUtils.GetDictionaryAsDataList(times);
        
        _times.Add(player.UserId, speedrunData);
    }

    public void SavePlayerTime(Player player)
    {
        var currentTime = _times[player.UserId];
        
        for (var index = 0; index < currentTime.Count; index++)
        {
            if (currentTime.Count <= index)
                continue;
            
            var currentIndex = currentTime[index];
            plugin.SaveDataHandler.SavePlayerTime(player, currentIndex.Speedrun, currentIndex.Elapsed);
        }
    }

    public void ClearOldCache()
    {
        var cached = _times.Keys;
        var playersOnline = Player.ReadyList.Select(player => player.UserId);
        var missingCachedPlayers = cached.Where(player => !playersOnline.Contains(player));

        foreach (var player in missingCachedPlayers)
        {
            _times.Remove(player);
        }
    }

    private static List<SpeedrunData> MakeEmpty()
    {
        var speedruns = Enum.GetValues(typeof(SpeedrunType)).Cast<SpeedrunType>();
        var speedrunDataList = new List<SpeedrunData>();

        foreach (var speedrun in speedruns)
        {
            var data = new SpeedrunData(speedrun, long.MaxValue);
            speedrunDataList.Add(data);
        }
        
        return speedrunDataList;
    }

    public List<SpeedrunData> GetSpeedrunData(Player player)
    {
        if (player.DoNotTrack || !_times.TryGetValue(player.UserId, out var data))
            return MakeEmpty();
        
        return data;
    }

    private List<SpeedrunData>? GetSpeedrunDataOrNull(Player player)
    {
        if (player.DoNotTrack || !_times.TryGetValue(player.UserId, out var data))
            return null;
        
        return data;
    }

    public long GetTime(SpeedrunType speedrun, Player player)
    {
        var dataList = GetSpeedrunDataOrNull(player);

        if (dataList == null || !dataList.TryGetFirst(targetData => targetData.Speedrun == speedrun, out var data))
            return long.MaxValue;
        
        return data.Elapsed;
    }
    
    public void SetTime(SpeedrunType speedrun, Player player, long newTime, bool force = false)
    {
        if (player.DoNotTrack)
            return;
        
        var speedrunData = GetSpeedrunDataOrNull(player) ?? MakeEmpty();
        var index = (int) speedrun;
        
        if (!force && speedrunData[index].Elapsed < newTime)
            return;
        
        speedrunData[index] = new SpeedrunData(speedrun, newTime);
        _times[player.UserId] = speedrunData;
    }
}
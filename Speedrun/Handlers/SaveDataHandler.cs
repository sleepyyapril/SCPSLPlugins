#nullable enable
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GameCore;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using Mono.Data.Sqlite;
using Speedrun.Enums;

namespace Speedrun.Handlers;

public class SaveDataHandler(SpeedrunPlugin plugin)
{
    private SpeedrunPlugin _plugin = plugin;
    private SqliteConnection? _connection;
    
    public bool Initialize()
    {
        var databaseFile = _plugin.GetDatabaseFile();
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = _plugin.GetPathForFile(databaseFile)
        }.ToString();
        
        _connection = new SqliteConnection(connectionString);
        
        try
        {
            _connection.Open();
        }
        catch (SqliteException exception)
        {
            Logger.Error(exception);
            return false;
        }

        CreateTables();
        return true;
    }

    public void Dispose()
    {
        if (_connection == null)
            return;
        
        _connection.Close();
        _connection.Dispose();
    }

    public void CreateTables()
    {
        Task.Run(async () =>
        {
            var tables = Enum.GetNames(typeof(SpeedrunType));

            foreach (var table in tables.Select(t => t.ToLower()))
            {
                await CreateTableAsync(table);
            }
        });
    }

    public async Task CreateTableAsync(string tableName)
    {
        if (_connection == null)
            return;

        var correctedTableName = $"{tableName}_table";
        
        using var command = _connection.CreateCommand();
        command.CommandText = $"CREATE TABLE IF NOT EXISTS `{correctedTableName}` (steam_id text unique, elapsed integer);";

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (DbException exception)
        {
            Logger.Error(exception);
        }
    }
    
    private string GetTableName(SpeedrunType type)
    {
        var name = Enum.GetName(typeof(SpeedrunType), type);
        return $"{name}_table" ?? throw new InvalidOperationException();
    }

    public void EnforceDoNotTrack(Player player)
    {
        if (!player.DoNotTrack)
            return;
        
        Task.Run(async () =>
        {
            await EnforceDoNotTrackAsync(player);
        });
    }

    private async Task EnforceDoNotTrackAsync(Player player)
    {
        if (_connection == null)
            return;
        
        var tables =  Enum.GetNames(typeof(SpeedrunType));
        using var transaction = _connection.BeginTransaction();
        
        foreach (var table in tables.Select(t => t.ToLower()))
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM {table} WHERE steam_id = @steam_id;";
            command.Parameters.AddWithValue("@steam_id", player.UserId);
            await command.ExecuteNonQueryAsync();
        }

        transaction.Commit();
    }

    public void SavePlayerTime(Player player, SpeedrunType speedrun, long elapsed)
    {
        if (player.DoNotTrack)
            return;
        
        Task.Run(async () =>
        {
            await SavePlayerTimeAsync(player, speedrun, elapsed);
        });
    }

    private async Task SavePlayerTimeAsync(Player player, SpeedrunType speedrun, long elapsed)
    {
        if (_connection == null)
            return;
        
        var tableName = GetTableName(speedrun);
        
        using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT OR REPLACE INTO {tableName} (steam_id, elapsed) VALUES (@steam_id, @elapsed);";
        command.Parameters.AddWithValue("@steam_id", player.UserId);
        command.Parameters.AddWithValue("@elapsed", elapsed);
        await command.ExecuteNonQueryAsync();
    }

    public long LoadPlayerTime(Player player, SpeedrunType speedrun)
    {
        if (player.DoNotTrack || _connection == null)
            return long.MaxValue;
        
        var tableName = GetTableName(speedrun);
        var elapsed = long.MaxValue;
        
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT elapsed FROM {tableName} WHERE steam_id=@steam_id;";
        command.Parameters.AddWithValue("@steam_id", player.UserId);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            elapsed = reader.GetInt64(0);
        }

        return elapsed;
    }
    
    public Dictionary<SpeedrunType, long> LoadPlayerTimes(Player player)
    {
        if (_connection == null)
            throw new NullReferenceException();
        
        var times = new Dictionary<SpeedrunType, long>();
        var types = Enum.GetValues(typeof(SpeedrunType)).Cast<SpeedrunType>();
        using var transaction = _connection.BeginTransaction();

        foreach (var type in types)
        {
            if (player.DoNotTrack || _connection == null)
            {
                times.Add(type, long.MaxValue);
                continue;
            }
            
            var tableName = GetTableName(type);
            var elapsed = long.MaxValue;
        
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT elapsed FROM {tableName} WHERE steam_id=@steam_id;";
            command.Parameters.AddWithValue("@steam_id", player.UserId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                elapsed = reader.GetInt64(0);
            }
            
            times.Add(type, elapsed);
        }
        
        transaction.Commit();
        return times;
    }
}
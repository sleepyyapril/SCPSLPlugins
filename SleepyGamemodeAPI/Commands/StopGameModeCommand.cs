using System;
using CommandSystem;
using SleepyGameModeAPI.Managers;

namespace SleepyGameModeAPI.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public class StopGameModeCommand : ICommand
{
    public string Command => "stopgamemode";
    public string[] Aliases => ["stopgm"];
    public string Description => "Starts a game mode";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.RoundEvents))
        {
            response = "You require RoundEvents to manage game modes.";
            return false;
        }
        
        GameModeManager.StopGameMode();
        
        response = "Current game mode stopped.";
        return true;
    }
}
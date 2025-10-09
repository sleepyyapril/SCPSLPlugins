using System;
using CommandSystem;
using SleepyGameModeAPI.Managers;

namespace SleepyGameModeAPI.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public class StartGameModeCommand : ICommand
{
    public string Command => "startgamemode";
    public string[] Aliases => [];
    public string Description => "Starts a game mode";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.RoundEvents))
        {
            response = "You require RoundEvents to manage game modes.";
            return false;
        }

        if (arguments.Count < 1)
        {
            response = "You must specify a game mode.";
            return false;
        }
        
        var gameMode = arguments.At(0);
        GameModeManager.StartGameMode(gameMode);
        
        response = "Game mode started.";
        return true;
    }
}
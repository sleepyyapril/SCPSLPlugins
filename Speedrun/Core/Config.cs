#nullable enable
namespace Speedrun.Core;

public class Config
{
    public string DatabaseFile { get; set; } = "speedrun.db";
    public bool UseGlobalDatabase { get; set; }
}
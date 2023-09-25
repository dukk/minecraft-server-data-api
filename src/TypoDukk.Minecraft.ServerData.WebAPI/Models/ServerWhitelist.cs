namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class ServerWhitelist
{
    public ServerWhitelist(string[] players)
    {
        this.Players = players;
        this.Timestamp = DateTime.UtcNow;
    }

    public string[] Players { get; }

    public DateTime Timestamp { get; }
}

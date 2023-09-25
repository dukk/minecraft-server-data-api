namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class ServerList
{
    public ServerList(string[] servers)
    {
        this.Servers = servers;
        this.Timestamp = DateTime.UtcNow;
    }

    public string[] Servers { get; }

    public DateTime Timestamp { get; }
}

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class ServerWarps
{
    public ServerWarps(string[] warps)
    {
        this.Warps = warps;
        this.Timestamp = DateTime.UtcNow;
    }

    public string[] Warps { get; }

    public DateTime Timestamp { get; }
}

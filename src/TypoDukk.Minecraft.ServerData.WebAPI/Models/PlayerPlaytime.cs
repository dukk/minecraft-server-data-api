namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class PlayerPlaytime
{
    public PlayerPlaytime(string playtime)
    {
        this.Playtime = playtime;
        this.Timestamp = DateTime.UtcNow;
    }

    public DateTime Timestamp { get; }

    public string Playtime { get; }
}

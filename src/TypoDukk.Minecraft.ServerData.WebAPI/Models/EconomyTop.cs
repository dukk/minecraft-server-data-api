namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class EconomyTop
{
    public EconomyTop(PlayerBalance[] topPlayers)
    {
        this.TopPlayers = topPlayers;
        this.Timestamp = DateTime.UtcNow;
    }

    public PlayerBalance[] TopPlayers { get; private set; }

    public DateTime Timestamp { get; private set; }
}

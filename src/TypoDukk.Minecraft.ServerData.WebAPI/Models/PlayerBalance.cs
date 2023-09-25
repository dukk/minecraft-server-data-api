namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public class PlayerBalance
{
    public PlayerBalance(string player, string balance, bool supressTimestamp = false)
    {
        this.Player = player;
        this.Balance = balance;

        if (!supressTimestamp)
            this.Timestamp = DateTime.UtcNow;
    }

    public string Player { get; }

    public string Balance { get; }

    public DateTime? Timestamp { get; }
}

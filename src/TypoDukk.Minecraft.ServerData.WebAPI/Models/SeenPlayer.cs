using System.Text.Json.Serialization;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

public enum SeenStatus
{
    Unknown,
    Online,
    Offline,
    Never
}

public class SeenPlayer
{
    public SeenPlayer(SeenStatus status, string? seenFor = null)
    {
        this.Status = status;
        this.For = seenFor;
        this.Timestamp = DateTime.UtcNow;
    }

    public SeenStatus Status { get; private set; }

    public string? For { get; private set; }

    public DateTime Timestamp { get; }
}

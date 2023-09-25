using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public interface IPlayerService
{
    Task<SeenPlayer> SeenPlayer(string server, string player);

    Task<PlayerPlaytime> GetPlayerPlaytime(string server, string player);
}

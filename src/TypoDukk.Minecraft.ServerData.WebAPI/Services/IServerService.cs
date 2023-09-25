using CoreRCON.Parsers.Standard;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public interface IServerService
{
    ServerList GetServerList();

    Task<string> GetBanList(string server);

    Task<ServerWarps> GetWarpsList(string server);

    Task<ServerWhitelist> GetWhitelist(string server);
}

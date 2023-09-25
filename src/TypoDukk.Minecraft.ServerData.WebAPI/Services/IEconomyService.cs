using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public interface IEconomyService
{
    Task<PlayerBalance[]> GetTopBalancesAsync(string server, int topN = 20);

    Task<string> GetPlayerBalanceAsync(string server, string player);
}

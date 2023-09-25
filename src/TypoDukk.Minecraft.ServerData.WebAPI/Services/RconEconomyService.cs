using CoreRCON;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text.RegularExpressions;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public class RconEconomyService : IEconomyService
{
    private readonly ILogger<RconEconomyService> logger;
    private readonly IRconService rconService;
    private readonly IValidationService validationService;

    public RconEconomyService(ILogger<RconEconomyService> logger, IRconService rconService, IValidationService validationService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.rconService = rconService ?? throw new ArgumentNullException(nameof(rconService));
        this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService)); ;
    }

    public async Task<string> GetPlayerBalanceAsync(string server, string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Illegal player name.", nameof(player));

        string result = await this.rconService.SendCommandAsync(server, $"balance {player}");

        var match = Regex.Matches(result, @"\s{1}(\.?[a-zA-Z0-9_]{2,16}): (.*?)$", RegexOptions.Singleline).Single();

        var resultPlayer = match.Groups[1].Value;

        if (!resultPlayer.Equals(player, StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException($"Player '{player}' and result player '{resultPlayer}' missmatch.");

        var resultBalance = match.Groups[2].Value;

        return resultBalance;
    }

    public async Task<PlayerBalance[]> GetTopBalancesAsync(string server, int maxPlayers = 20)
    {
        return await this.GetTopBalancesAsync(server, maxPlayers: maxPlayers, loopHackCount: 0);
    }

    public async Task<PlayerBalance[]> GetTopBalancesAsync(string server, int maxPlayers = 20, int loopHackCount = 0)
    {
        this.logger.LogDebug("GetTopBalancesAsync: Getting top balances from {server} with maxPlayers: {maxPlayers} [loopHackCount: {loopHackCount}].", server, maxPlayers, loopHackCount);

        var commands = new List<string>();
        var pageCount = Convert.ToInt32(Math.Ceiling(maxPlayers / 8d));

        foreach (var index in Enumerable.Range(1, pageCount))
            commands.Add($"balancetop {index}");

        var results = await this.rconService.SendCommandSeriesAsync(server, true, commands.ToArray());
        var allResults = String.Join(Environment.NewLine, results);

        // dukk: This is a hack because for some reason the first time around this was always blank...
        if (loopHackCount < 5 && String.IsNullOrWhiteSpace(allResults))
        {
            this.logger.LogWarning("GetTopBalancesAsync: Triggered loop hack [loopHackCount: {loopHackCount}].", loopHackCount);
            return await this.GetTopBalancesAsync(server, 
                maxPlayers: maxPlayers,
                loopHackCount: ++loopHackCount); // stop infinent loop
        }

        var matches = Regex.Matches(allResults, @"^\d*\. (.*?), (.*)\w*$", RegexOptions.Multiline);

        var topPlayers = new List<PlayerBalance>((from m in matches select new PlayerBalance(m.Groups[1].Value, m.Groups[2].Value.TrimEnd(), true)).Take(maxPlayers));

        return topPlayers.ToArray();
    }
}

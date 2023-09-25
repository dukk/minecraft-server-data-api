using System.Text.RegularExpressions;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public class RconPlayerService : IPlayerService
{
    private readonly ILogger<RconPlayerService> logger;
    private readonly IRconService rconService;
    private readonly IValidationService validationService;

    public RconPlayerService(ILogger<RconPlayerService> logger, IRconService rconService, IValidationService validationService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.rconService = rconService ?? throw new ArgumentNullException(nameof(rconService));
        this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService)); ;
    }

    public async Task<PlayerPlaytime> GetPlayerPlaytime(string server, string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Invalid player.", nameof(player));

        string result = await this.rconService.SendCommandAsync(server, $"playtime {player}");

        if (result.StartsWith("Error"))
            throw new ArgumentException("Unknown player", nameof(player));

        var playtime = result.Substring(result.IndexOf(':') + 1).Trim();

        return new PlayerPlaytime(playtime);
    }

    public async Task<SeenPlayer> SeenPlayer(string server, string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Invalid player.", nameof(player));

        string result = await this.rconService.SendCommandAsync(server, $"seen {player}");
        SeenStatus status;
        string? seenFor = null;

        try
        {
            if (!string.IsNullOrWhiteSpace(result))
            {
                status = result.Contains("offline")
                    ? SeenStatus.Offline
                    : SeenStatus.Online;

                const string since = "since";
                var indexOfSince = result.IndexOf(since);

                if (indexOfSince > 0)
                {
                    var start = indexOfSince + since.Length + 1;
                    var indexOfNewLine = result.IndexOf("\n");

                    seenFor = result.Substring(start, indexOfNewLine - start).TrimEnd('\n', '\r', '.');
                }
            }
            else
            {
                status = SeenStatus.Never;
            }
        }
        catch
        {
            status = SeenStatus.Unknown;
        }

        return new SeenPlayer(status, seenFor);
    }
}
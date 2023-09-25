using System.Text.RegularExpressions;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public class RconServerService : IServerService
{
    private readonly ILogger<RconServerService> logger;
    private readonly IRconService rconService;
    private readonly RconConfiguration rconConfiguration;

    public RconServerService(ILogger<RconServerService> logger, IRconService rconService, RconConfiguration rconConfiguration)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.rconService = rconService ?? throw new ArgumentNullException(nameof(rconService));
        this.rconConfiguration = rconConfiguration ?? throw new ArgumentNullException(nameof(rconConfiguration)); ;
    }
    public ServerList GetServerList()
    {
        var servers = new List<string>(from s in this.rconConfiguration.Servers select s.Name);

        servers.Sort();

        return new ServerList(servers.ToArray());
    }

    public async Task<string> GetBanList(string server)
    {
        /*  
        There are no bans

        There are 8 ban(s):
        AAA was banned by Console: The Ban Hammer has spoken!
        BBB was banned by Console: The Ban Hammer has spoken!
        CCC was banned by Console: The Ban Hammer has spoken!
        DDD was banned by Console: The Ban Hammer has spoken!
        EEE was banned by Console: The Ban Hammer has spoken!
        FFF was banned by Console: The Ban Hammer has spoken!
        GGG was banned by Console: The Ban Hammer has spoken!
        HHH was banned by Console: The Ban Hammer has spoken!
        */
        string result = await this.rconService.SendCommandAsync(server, $"banlist"); // RCON returns all of these without line breaks...


        return result;
    }

    public async Task<ServerWarps> GetWarpsList(string server)
    {
        string result = await this.rconService.SendCommandAsync(server, $"warp list");
        var resultParts = result.Split(':');
        var warps = resultParts[1].Split(',');

        for (int i = 0; i < warps.Length; i++)
            warps[i] = warps[i].Trim();

        var warpsList = new List<string>(warps);

        warpsList.Sort();

        return new ServerWarps(warpsList.ToArray());
    }

    public async Task<ServerWhitelist> GetWhitelist(string server)
    {
        string result = await this.rconService.SendCommandAsync(server, $"whitelist list");
        var resultParts = result.Split(':');

        var matches = Regex.Matches(resultParts[1], @"\s?([a-zA-Z0-9_]{2,16}),?", RegexOptions.Multiline);

        var players = new List<string>(from m in matches select m.Groups[1].Value);

        players.Sort();

        return new ServerWhitelist(players.ToArray());
    }
}

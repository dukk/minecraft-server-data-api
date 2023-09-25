using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Controllers;

[ApiController]
[Route("servers")]
public class ServersController : ControllerBase
{
    private readonly ILogger<ServersController> logger;
    private readonly RconConfiguration rconConfiguration;
    private readonly IServerService serverService;

    public ServersController(ILogger<ServersController> logger, RconConfiguration rconConfiguration, IServerService serverService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.rconConfiguration = rconConfiguration ?? throw new ArgumentNullException(nameof(rconConfiguration));
        this.serverService = serverService ?? throw new ArgumentNullException(nameof(serverService));
    }

    [HttpGet("", Name = "ServerList")]
    public ServerList GetServerList()
    {
        return this.serverService.GetServerList();
    }

    [HttpGet("{server}/bans", Name = "GetBanList")]
    public async Task<string> GetBanList([FromRoute] string server)
    {
        return await this.serverService.GetBanList(server);
    }

    [HttpGet("{server}/warps", Name = "GetWarpsList")]
    public async Task<ServerWarps> GetWarpsList([FromRoute] string server)
    {
        return await this.serverService.GetWarpsList(server);
    }

    [HttpGet("{server}/whitelist", Name = "GetWhitelist")]
    public async Task<ServerWhitelist> GetWhitelist([FromRoute] string server)
    {
        return await this.serverService.GetWhitelist(server);
    }
}

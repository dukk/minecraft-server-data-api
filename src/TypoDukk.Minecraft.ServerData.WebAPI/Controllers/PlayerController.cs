using Microsoft.AspNetCore.Mvc;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Controllers;

[ApiController]
[Route("servers/{server}/players")]
public class PlayerController : ControllerBase
{
    private readonly ILogger<PlayerController> logger;
    private readonly IPlayerService playerService;
    private readonly IValidationService validationService;

    public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService, IValidationService validationService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        ;
    }

    [HttpGet("{player}/seen", Name = "SeenPlayer")]
    public async Task<SeenPlayer> SeenPlayer([FromRoute] string server, [FromRoute] string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Invalid player.", nameof(player));

        var result = await this.playerService.SeenPlayer(server, player);

        return result;
    }

    [HttpGet("{player}/playtime", Name = "GetPlayerPlaytime")]
    public async Task<PlayerPlaytime> GetPlayerPlaytime([FromRoute] string server, [FromRoute] string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Invalid player.", nameof(player));

        var result = await this.playerService.GetPlayerPlaytime(server, player);

        return result;
    }
}

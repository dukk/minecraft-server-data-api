using Microsoft.AspNetCore.Mvc;
using System.Net;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Models;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Controllers;

[ApiController]
[Route("economy/{server}")]
public class EconomyController : ControllerBase
{
    private readonly ILogger<EconomyController> logger;
    private readonly IEconomyService economyService;
    private readonly IValidationService validationService;

    public EconomyController(ILogger<EconomyController> logger, IEconomyService economyService, IValidationService validationService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.economyService = economyService ?? throw new ArgumentNullException(nameof(economyService));
        this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    [HttpGet("balance/top", Name = "GetEconomyTop")]
    public async Task<EconomyTop> GetEconomyTop([FromRoute] string server, [FromQuery] int maxPlayers = 5)
    {
        if (maxPlayers > 20) // TODO: dukk: move this to a configuration value
            maxPlayers = 20;

        var results = await this.economyService.GetTopBalancesAsync(server, maxPlayers);

        return new EconomyTop(results);
    }

    [HttpGet("balance/players/{player}", Name = "GetPlayerBalance")]
    public async Task<PlayerBalance> GetPlayerBalance([FromRoute] string server, [FromRoute] string player)
    {
        if (!this.validationService.IsValidPlayer(player))
            throw new ArgumentException("Invalid player.", nameof(player));

        var balance = await this.economyService.GetPlayerBalanceAsync(server, player);

        return new PlayerBalance(player, balance);
    }
}
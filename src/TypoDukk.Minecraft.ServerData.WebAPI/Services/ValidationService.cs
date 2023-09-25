using System.Text.RegularExpressions;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public class ValidationService : IValidationService
{
    private readonly ILogger<ValidationService> logger;

    public ValidationService(ILogger<ValidationService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool IsValidPlayer(string player)
    {
        return Regex.IsMatch(player, @"^\.?[a-zA-Z0-9_]{2,16}$");
    }
}
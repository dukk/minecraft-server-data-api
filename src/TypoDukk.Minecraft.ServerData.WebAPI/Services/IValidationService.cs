using CoreRCON.Parsers.Standard;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public interface IValidationService
{
    public bool IsValidPlayer(string playerName);
}

using Microsoft.AspNetCore.Hosting.Server;
using System.Net;
using System.Xml.Linq;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public interface IRconService
{
    Task<string> SendCommandAsync(string server, string command, bool stripColor = true);

    Task<string[]> SendCommandSeriesAsync(string server, bool stripColor = true, params string[] command);
}

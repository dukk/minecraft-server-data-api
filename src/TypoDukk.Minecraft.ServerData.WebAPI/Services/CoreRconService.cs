using CoreRCON;
using System.Diagnostics;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

public class CoreRconService : IRconService
{
    private readonly RconConfiguration rconConfiguration;

    public CoreRconService(RconConfiguration rconConfiguration)
    {
        this.rconConfiguration = rconConfiguration ?? throw new ArgumentNullException(nameof(rconConfiguration));
    }

    public async Task<string> SendCommandAsync(string server, string command, bool stripColor = true)
    {
        var result = await this.withRcon(server, (rcon) => rcon.SendCommandAsync(command));

        return stripColor
            ? this.stripColor(result)
            : result;
    }

    public async Task<string[]> SendCommandSeriesAsync(string server, bool stripColor = true, params string[] commands)
    {
        return await this.withRcon(server, async (rcon) =>
        {
            var resultList = new List<string>(commands.Length);

            foreach (var command in commands)
            {
                var result = await rcon.SendCommandAsync(command);

                await Task.Delay(250); // dukk: adding a delay because I was getting locking exceptions on the server

                resultList.Add(stripColor 
                                ? this.stripColor(result) 
                                : result);
            }

            return resultList.ToArray();
        });
    }

    private async Task<T> withRcon<T>(string server, Func<RCON, Task<T>> action, bool connect = true)
    {
        var serverConfig = this.rconConfiguration.GetServerByName(server);

        if (serverConfig.Host is null)
            throw new ApplicationException($"Missing Rcon:Server:Host configuration for server name '{server}'.");

        if (serverConfig.Port is null)
            throw new ApplicationException($"Missing Rcon:Server:Port configuration for server name '{server}'.");

        if (serverConfig.Password is null)
            throw new ApplicationException($"Missing Rcon:Server:Password configuration for server name '{server}'.");

        using (var rcon = new RCON(serverConfig.GetIPAddressFromHost(), serverConfig.Port.Value, serverConfig.Password))
        {
            if (connect)
                await rcon.ConnectAsync();

            await Task.Delay(250); // dukk: adding a delay because I was getting locking exceptions on the server

            return await action(rcon);
        }
    }

    private string stripColor(string result)
    {
        using (var writer = new StringWriter())
        {
            for (var index = 0; index < result.Length; index++)
            {
                char c = result[index];

                if (c == 167)
                {
                    index++;
                    continue;
                }
             
                writer.Write(c);
            }

            return writer.ToString();
        }
    }
}

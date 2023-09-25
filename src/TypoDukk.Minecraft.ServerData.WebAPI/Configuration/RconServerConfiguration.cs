using Microsoft.AspNetCore.Hosting.Server;
using System.Net;

namespace TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;

public class RconServerConfiguration
{
    public string? Name { get; set; }

    public string? Host {  get; set; }
    
    public ushort? Port { get; set; }

    public string? Password { get; set; }

    public IPAddress GetIPAddressFromHost()
    {
        if (this.Host is null)
            throw new ApplicationException("Unable to Get IP Address, missing Rcon:Server:Host configuration.");

        if (IPAddress.TryParse(this.Host, out var ipAddress))
            return ipAddress;

        return Dns.GetHostAddresses(this.Host).First();
    }
}
